using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using refShop_DEV.Models.MyDbContext;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using refShop_DEV.Models.Login;
using refShop_DEV.Models.Permission;
using refShop_DEV.Models;
using refShop_DEV.Services.Interfaces;
using refShop_DEV.Services;

namespace refShop_DEV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly AuthenticationServiceJWT _authService;

        public UserController(MyDbContext context, IMapper mapper, IConfiguration configuration, AuthenticationServiceJWT authService)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticationRequest form, [FromServices] IDTOInterfaces dTOInterfaces)
        {
            var existingUser = await _context.Users
             .Include(u => u.UserRole)
                 .ThenInclude(ur => ur.RolePermissions)
                     .ThenInclude(rp => rp.Permission)
                     .Include(u => u.Turno)
             .FirstOrDefaultAsync(u => u.Username == form.Username);

            //var existingUser = await _context.Users.FirstOrDefaultAsync();

            if (existingUser == null)
            {
                return BadRequest("Usuario y/o contraseña incorrectos");
            }

            if (!VerifyPasswordHash(form.Password, existingUser.PasswordHash))
            {
                return BadRequest("Usuario y/o contraseña incorrectos");
            }

            var log = new Log
            {
                TableName = "Users",
                RecordId = existingUser.Id,
                Operation = "Login",
                UserId = existingUser.Id,
                ColumnName = "",
                OldValue = "",
                NewValue = "",
                Description = "LOGIN DE USUARIO",
                Timestamp = DateTime.UtcNow
            };

            var userRoles = _mapper.Map<UserRoleDto>(existingUser.UserRole);
            // Mapeo de RolePermissions a RolePermissionsDTO
            var rolePermissionsDto = _mapper.Map<List<RolePermissionsDTO>>(existingUser.UserRole.RolePermissions);

            // Mapeo de ActivityPermission a PermissionDTO
            var permissionDto = _mapper.Map<List<PermissionDTO>>(existingUser.UserRole.RolePermissions.Select(rp => rp.Permission));

            dTOInterfaces.UserRole = userRoles;
            dTOInterfaces.Permissions = permissionDto.Cast<IPermissionsDTO>().ToList();

            var token = _authService.GenerateToken(existingUser,permissionDto);
            existingUser.CreatedAt = existingUser.CreatedAt;
            existingUser.UpdatedAt = existingUser.UpdatedAt;

            

            var responseDto = _mapper.Map<UserDto>(existingUser);
            _context.Logs.Add(log);
            var registroActividadUpdater = new RegistroActividadUpdater(_context, existingUser);
            registroActividadUpdater.Start();
            await _context.SaveChangesAsync();


            return Ok(new { token, user = responseDto/*, rolePermissions = rolePermissionsDto, permissions = permissionDto */});
        }


        private bool VerifyPasswordHash(string password, string passwordHash)
        {
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(null, passwordHash, password);

            return result == PasswordVerificationResult.Success;
        }

    }


}
