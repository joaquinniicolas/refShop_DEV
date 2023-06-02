using refShop_DEV.Models.Login;

namespace refShop_DEV.Models.Restaurant
{
    public class Pedido
    {
        public int Id { get; set; }
        public int IdPlato { get; set; }
        public int IdMesa { get; set; }
        public int IdUsuario { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaPedido { get; set; }
        public string EstadoPedido { get; set; }
        public float Total { get; set; }
        public Plato IdPlatoNavigation { get; set; }
        public Mesa IdMesaNavigation { get; set; }
        public User IdUsuarioNavigation { get; set; }
    }
}
