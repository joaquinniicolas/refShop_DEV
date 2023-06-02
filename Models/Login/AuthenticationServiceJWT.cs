using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace refShop_DEV.Models.Login
{

    public class AuthenticationServiceJWT
    {
        private readonly SymmetricSecurityKey _signingKey;
        private readonly IConfiguration _configuration;

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

        public string GenerateToken(User user)
        {
            var roleName = user.UserRole != null ? user.UserRole.Name : "default_role";
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName + user.LastName), // Agregado para incluir el nombre del usuario
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                ValidateIssuer = true,
                ValidIssuer = "tu_issuer",
                ValidateAudience = true,
                ValidAudience = "tu_audience",
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
