using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using refShop_DEV.Models.Login;

namespace refShop_DEV.Models.Permission
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Range(1, 999)]
        public int Level { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<RolePermissions> RolePermissions { get; set; }
    }







}
