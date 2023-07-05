using Microsoft.Extensions.Logging;
using refShop_DEV.Models.Login;
using System.Security.Claims;

namespace refShop_DEV.Services
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenValidationMiddleware> _logger;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<TokenValidationMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;

        }

        public async Task Invoke(HttpContext context, AuthenticationServiceJWT authService)
        {
            var authorizationHeaderExists = context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader);

            if (authorizationHeaderExists && !string.IsNullOrEmpty(authorizationHeader))
            {
                var token = authorizationHeader.FirstOrDefault()?.Split(" ").Last();

                ClaimsPrincipal principal = authService.ValidateToken(token);

                if (principal != null)
                {
                    // La validación del token fue exitosa
                    context.User = principal;
                    _logger.LogInformation("Token validation successful.");
                    _logger.LogInformation("Token: {Token}", token);
                    _logger.LogInformation("Principal: {@Principal}", principal);


                }
                else
                {
                    _logger.LogWarning("Token validation failed.");

                }
            }
            else {
                _logger.LogWarning("Token not found in the request.");

            }

            await _next(context);
        }
    }

}
