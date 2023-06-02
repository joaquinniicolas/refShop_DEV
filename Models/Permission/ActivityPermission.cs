using System.ComponentModel.DataAnnotations;

namespace refShop_DEV.Models.Permission
{
    public class ActivityPermission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public ICollection<RolePermissions> RolePermissions { get; set; }
    }
}
