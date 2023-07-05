using Microsoft.AspNetCore.Mvc;
using refShop_DEV.Services;

namespace refShop_DEV.Models.Permission
{
    public class PermissionAuthorizationFilterAttribute : TypeFilterAttribute
    {
        public PermissionAuthorizationFilterAttribute(string[] requiredPermission)
            : base(typeof(PermissionAuthorizationFilter))
        {
            Arguments = new object[] { requiredPermission };
        }
    }
}
