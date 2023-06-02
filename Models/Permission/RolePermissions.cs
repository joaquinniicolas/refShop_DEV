namespace refShop_DEV.Models.Permission
{
    public class RolePermissions
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public UserRole Role { get; set; }
        public ActivityPermission Permission { get; set; }
    }
}
