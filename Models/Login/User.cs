using refShop_DEV.Models.Permission;
using refShop_DEV.Models.Restaurant;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace refShop_DEV.Models.Login
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es requerido.")]
        [MaxLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [MaxLength(255, ErrorMessage = "La contraseña no puede tener más de 255 caracteres.")]
        [JsonIgnore]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; }

        [MaxLength(20, ErrorMessage = "El número de teléfono no puede tener más de 20 caracteres.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El rol de usuario es requerido.")]
        public int UserRoleId { get; set; }

        [MaxLength(255, ErrorMessage = "La ruta de la foto no puede tener más de 255 caracteres.")]
        public string PhotoPath { get; set; }

        [Required(ErrorMessage = "La fecha de creación es requerida.")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "La fecha de actualización es requerida.")]
        public DateTime UpdatedAt { get; set; }

        public virtual UserRole UserRole { get; set; }

        //PERMISSIONS

     

        public virtual ICollection<Log> Logs { get; set; }

        public virtual ICollection<Categoria> Categorias { get; set; }

        public ICollection<Mesa> MesasCreadas { get; set; }
        public ICollection<Mesa> MesasModificadas { get; set; }
        public ICollection<Mesa> MesasAsociadas { get; set; }
        public ICollection<Mesa> MesasMozo { get; set; }
        public virtual Turno? Turno { get; set; }



    }




}
