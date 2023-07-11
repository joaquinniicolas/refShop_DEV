using refShop_DEV.Models.Login;
using System.ComponentModel.DataAnnotations;

namespace refShop_DEV.Models.Restaurant
{

    public class RegistroActividad
    {
        [Key]
        public int ID_Registro { get; set; }
        public int ID_Empleado { get; set; }
        public int? IDTurno { get; set; }
        public DateTime Fecha_HoraInicio { get; set; }
        public DateTime? Fecha_HoraFin { get; set; }
        public bool Conectado { get; set; }
        public decimal Horas_Extras { get; set; }
        public decimal Horas_Faltantes { get; set; }
        public TimeSpan? Demora_Inicio { get; set; }
        public bool Justificado { get; set; }
        public string Comentario { get; set; }

        // Relaciones con otras entidades
        public virtual User User { get; set; }
        public virtual Turno Turno { get; set; }
    }


}
