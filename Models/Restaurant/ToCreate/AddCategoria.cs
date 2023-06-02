using refShop_DEV.Models.Login;

namespace refShop_DEV.Models.Restaurant.ToCreate
{
    public class AddCategoria
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime CreadoEl { get; set; }
        public int CreadoPor { get; set; }
        public int MostrarEnSitio { get; set; }
        public User CreadoPorNavigation { get; set; }
    }
}
