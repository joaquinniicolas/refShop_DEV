using refShop_DEV.Models.Restaurant;
using System.ComponentModel.DataAnnotations;

namespace refShop_DEV.Models.Login
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int UserRoleId { get; set; }
        public string PhotoPath { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        //public ICollection<Mesa> MesasCreadas { get; set; }
        //public ICollection<Mesa> MesasModificadas { get; set; }
        //public ICollection<Mesa> MesasAsociadas { get; set; }
        //public ICollection<Mesa> MesasMozo { get; set; }

        //public virtual UserRole UserRole { get; set; }
        //public virtual ICollection<Log> Logs { get; set; }

        //public virtual ICollection<Categoria> Categorias { get; set; }


    }



}
