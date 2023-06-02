using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace refShop_DEV.Models.Login
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(PasswordHash), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [DefaultValue(1)]
        public int RoleId { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

}
