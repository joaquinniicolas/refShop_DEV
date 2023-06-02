using refShop_DEV.Models.Login;
using System.ComponentModel.DataAnnotations.Schema;

namespace refShop_DEV.Models.Restaurant
{
    public class MesaDTO
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

        public string Creador { get; set; }

        public string Modificador { get; set; }

        public string MozoEncargado { get; set; }

    }
}
