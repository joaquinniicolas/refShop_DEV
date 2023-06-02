using refShop_DEV.Models.Login;

namespace refShop_DEV.Models.Restaurant
{
    public class Promocion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public float Descuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int AplicadoPor { get; set; }
        public bool Estado { get; set; }
        public User AplicadoPorNavigation { get; set; }
    }
}
