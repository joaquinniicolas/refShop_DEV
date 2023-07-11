using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace refShop_DEV.Models.Restaurant
{
    public class Turno
    {
        [Key]
        public int IDTurno { get; set; }
        public int? IDEmpleado { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public int? CreadoPor { get; set; }
        public string Dias_Laborales { get; set; }
        public string CargaHoraria { get; set; }

        // ... otras propiedades y relaciones ...
    }


}
