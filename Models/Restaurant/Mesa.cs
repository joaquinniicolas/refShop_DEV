using Newtonsoft.Json;
using refShop_DEV.Models.Login;
using System.ComponentModel.DataAnnotations.Schema;

namespace refShop_DEV.Models.Restaurant
{

    public class Mesa
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; }

        [Column("creado_el")]
        public DateTime CreadoEl { get; set; }

        [Column("creado_por")]
        public int CreadoPor { get; set; }

        [Column("modificado_el")]
        public DateTime ModificadoEl { get; set; }

        [Column("modificado_por")]
        public int ModificadoPor { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_mozo")]
        public int IdMozo { get; set; }

        public virtual User? CreadoPorNavigation { get; set; }
        public virtual User? ModificadoPorNavigation { get; set; }
        public virtual User? IdUsuarioNavigation { get; set; }
        public virtual User? IdMozoNavigation { get; set; }

        // Propiedades de navegación muchos a muchos adicionales
        [NotMapped]
        public ICollection<Mesa>? MesasCreadas { get; set; }
        [NotMapped]
        public ICollection<Mesa>? MesasModificadas { get; set; }
        [NotMapped]
        public ICollection<Mesa>? MesasAsociadas { get; set; }
        [NotMapped]
        public ICollection<Mesa>? MesasMozo { get; set; }


    }


}
