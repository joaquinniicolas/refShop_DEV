using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using refShop_DEV.Models.MyDbContext;
using refShop_DEV.Models.Permission;
using System.Security.Claims;

namespace refShop_DEV.Services
{
    public class PermissionAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly string[] _requiredPermissions;

        public PermissionAuthorizationFilter(string[] requiredPermissions)
        {
            _requiredPermissions = requiredPermissions;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated || _requiredPermissions.Length == 0)
            {
                context.Result = new ForbidResult();
                return;
            }

            var permissionsClaims = user.Claims.Where(c => c.Type == "Permission").ToList();

            if (permissionsClaims.Count > 0)
            {
                var userPermissions = permissionsClaims.SelectMany(c => c.Value.Split(',')).Distinct().ToArray();

                if (_requiredPermissions.Any(rp => userPermissions.Contains(rp)))
                {
                    // El usuario tiene al menos uno de los permisos requeridos
                    // Puedes continuar con la lógica adicional aquí
                    //context.Result = new OkResult();
                    return;
                }
                else
                {
                    // El usuario no tiene ninguno de los permisos requeridos
                    context.Result = new ForbidResult();
                    return;
                }
            }
            else
            {
                // No se encontró el claim "Permission" para el usuario
                context.Result = new ForbidResult();
                return;
            }

        }
    }




}

//private bool HasRequiredPermissions(AuthorizationFilterContext context)
//{

//}