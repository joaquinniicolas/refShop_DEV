using System.ComponentModel.DataAnnotations;

namespace refShop_DEV.Models.Login
{
    public class AuthenticationRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
