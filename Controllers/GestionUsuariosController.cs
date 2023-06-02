using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using refShop_DEV.Models.Login;
using refShop_DEV.Models.MyDbContext;
using refShop_DEV.Models.Permission;
using refShop_DEV.Models.Restaurant;
using System.Security.Claims;

namespace refShop_DEV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]

    public class GestionUsuariosController : ControllerBase
    {

        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserDto _authenticatedUser;
        private readonly RolePermissionsDTO _rolePermission;
        private readonly PermissionDTO _permissionDTO;
        public GestionUsuariosController(MyDbContext context, IMapper mapper, UserDto authenticatedUser, RolePermissionsDTO rolePermission, PermissionDTO permission)
        {
            _context = context;
            _mapper = mapper;
            _authenticatedUser = authenticatedUser;
            _rolePermission = rolePermission;
            _permissionDTO = permission;
            
        }

        [HttpGet("getRoles")]
        public ActionResult<IEnumerable<UserRoleDto>> GetRoles()
        {
            var roles = _context.UserRoles.ToList();
            if(roles.Count == 0)
            {
                return NotFound();
            }

            var rolesDto = _mapper.Map<List<UserRoleDto>>(roles);

            return Ok(rolesDto);
        }



        [HttpGet("getUsers")]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var user = HttpContext.User;
            bool isAdmin = user.IsInRole("Admin");


            var users = _context.Users.ToList();

            if (users.Count == 0)
            {
                return NotFound(); // No se encontraron usuarios
            }

            var userDtos = _mapper.Map<List<UserDto>>(users);

            return Ok(userDtos);
        }


        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto, string photoUrl)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerDto.UserName);

            if (existingUser != null)
            {
                return BadRequest("El usuario ya se encuentra registrado");
            }

            if (registerDto.RoleId == null)
            {
                registerDto.RoleId = 1; // Mozo level
            }
            else if (registerDto.RoleId != 1 && registerDto.RoleId != 2 && registerDto.RoleId != 3 && registerDto.RoleId != 4)
            {
                return BadRequest("Invalid role ID");
            }



            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Username = registerDto.UserName,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                UserRoleId = (int)registerDto.RoleId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            if(photoUrl != null)
            {
                newUser.PhotoPath = photoUrl;
            }
            else
            {
                newUser.PhotoPath = "Sin definir";
            }
            
            var passwordHasher = new PasswordHasher<User>();
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, registerDto.PasswordHash);

            _context.Users.Add(newUser);

            var log = new Log
            {
                TableName = "Users",
                RecordId = newUser.Id,
                Operation = "INSERT",
                UserId = newUser.Id,
                ColumnName = "ALL TABLE",
                OldValue = "-",
                NewValue = "ALL FIELD",
                Description = "CREACION DE USUARIO",
                Timestamp = DateTime.UtcNow
            };

            // Agregar el objeto Log al contexto de base de datos y guardar los cambios.
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            var responseDto = _mapper.Map<UserDto>(newUser);

            return Ok(responseDto);
        }




        [HttpPost("save")]
        public async Task<IActionResult> SaveUserPhoto(string username, IFormFile photo)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Profiles", "UserPhotos", username);

            // Crear la carpeta si no existe
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            var photoUrl = Path.Combine("Profiles", "UserPhotos", username, fileName).Replace("\\", "/");
            var response = new { photoUrl }; // Crear un objeto anónimo con la propiedad "photoUrl"
            return Ok(response);
        }
       


        [HttpGet("checkUserName")]
        public ActionResult<bool> CheckUserName(string userName)
        {
            bool userExists = _context.Users.Any(u => u.Username == userName);

            return userExists;
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            // Buscar el usuario en la base de datos por su ID
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(); // Usuario no encontrado
            }

            // Mapear el usuario encontrado a DTO
            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }


        [HttpGet("getUser")]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized(); // El usuario no está autenticado o la claim no está presente
            }

            var userId = int.Parse(userIdClaim.Value);

            // Obtener el usuario de la base de datos utilizando el ID obtenido

            User? user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(); // Usuario no encontrado
            }

            var responseDto = _mapper.Map<UserDto>(user);


            return Ok(responseDto);
        }

        [HttpGet("getUsersByRole")]
        public ActionResult<IEnumerable<UserDto>> GetUsersByRole()
        {
            var mozoRoleId = 1; // ID del rol "Mozo"

            var users = _context.Users.Where(u => u.UserRoleId == mozoRoleId).ToList();

            if (users.Count == 0)
            {
                return NotFound(); // No se encontraron usuarios con el rol "Mozo"
            }

            var userDtos = _mapper.Map<List<UserDto>>(users);

            return Ok(userDtos);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            // Verificar si el ID del usuario proporcionado coincide con el ID del DTO
            if (id != userDto.Id)
            {
                return BadRequest(); // ID no coincide, solicitud inválida
            }

            // Buscar el usuario en la base de datos por su ID
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(); // Usuario no encontrado
            }


            // Actualizar las propiedades del usuario con los valores del DTO
            // Ten en cuenta que esto puede variar dependiendo de la estructura de tu modelo de datos
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Username = userDto.UserName;
            user.Email = userDto.Email;
            user.Phone = userDto.Phone;
            user.PhotoPath = userDto.PhotoPath;
            user.UpdatedAt = new DateTime();
            user.UserRole.Id = userDto.UserRoleId;

            var oldValue = JsonConvert.SerializeObject(userDto);
            var newValue = JsonConvert.SerializeObject(user);

            var log = new Log
            {
                TableName = "User",
                RecordId = userDto.Id,
                Operation = "UPDATE",
                UserId = GetUserId(),
                ColumnName = "-",
                OldValue = oldValue,
                NewValue = newValue,
                Description = "Se ha actualizado un Usuario",
                Timestamp = DateTime.UtcNow
            };

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return NoContent(); // Sin contenido, edición exitosa
        }

        private int GetUserId()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);
            }
            throw new Exception("User ID not found.");


        }

        


    }
}
