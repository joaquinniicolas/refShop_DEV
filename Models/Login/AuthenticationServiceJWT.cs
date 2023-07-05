using Microsoft.IdentityModel.Tokens;
using refShop_DEV.Models.Permission;
using refShop_DEV.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace refShop_DEV.Models.Login
{

    public class AuthenticationServiceJWT : ITokenService
    {
        private readonly SymmetricSecurityKey _signingKey;
        private readonly IConfiguration _configuration;
        private static User _user;
        private static List<PermissionDTO> _permissions;
        private DateTime tokenExpiration;

        public AuthenticationServiceJWT(IConfiguration configuration)
        {
            _configuration = configuration;
            var jwtKey = configuration.GetValue<string>("Jwt:Key");
            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new ArgumentException("El valor de 'JwtSettings:Key' no puede ser nulo o vacío.");
            }
            
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
        }

        //PARA GENERAR EL TOKEN DE AUTHENTICACION
        public string GenerateToken(User user, List<PermissionDTO> permissions)
        {
            _user = user;
            _permissions = permissions;
            var roleName = user.UserRole != null ? user.UserRole.Name : "default_role";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName + user.LastName), // Agregado para incluir el nombre del usuario
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName),

            };

            // Agregar los permisos como claims
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission.Name));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256));
            tokenExpiration = DateTime.UtcNow.AddMinutes(30);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //PARA RENOVAR EL TOKEN
        public string RenewToken()
        {
            var roleName = _user.UserRole != null ? _user.UserRole.Name : "default_role";
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
            new Claim(ClaimTypes.Name, _user.FirstName + _user.LastName),
            new Claim(ClaimTypes.Email, _user.Email),
            new Claim(ClaimTypes.Role, roleName),
        };

            // Agregar los permisos como claims
            foreach (var permission in _permissions)
            {
                claims.Add(new Claim("Permission", permission.Name));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: tokenExpiration.AddMinutes(30), // Renovar el token por otros 30 minutos
                signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256));
            tokenExpiration = tokenExpiration.AddMinutes(30); // Renovar el token por otros 30 minutos ACT: SE IMPLEMENTO METODOLOGIA DE LLAMADO DE API EN ANGULAR

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public ClaimsPrincipal ValidateToken(string token)
        {
            Console.WriteLine("Token: " + token); // Imprimir el token en la consola

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return principal;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

       
        
    }

}
