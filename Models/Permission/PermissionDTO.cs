using refShop_DEV.Services.Interfaces;

namespace refShop_DEV.Models.Permission
{
    public class PermissionDTO : IPermissionsDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }


    }
}
