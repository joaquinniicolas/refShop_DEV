using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace refShop_DEV.Models.Permission
{
    public class RolePermissions
    {
        [Key]
        public int RoleId { get; set; }

        [Key]
        public int PermissionId { get; set; }

        public UserRole UserRole { get; set; }
        public Permissions Permission { get; set; }
    }

}
