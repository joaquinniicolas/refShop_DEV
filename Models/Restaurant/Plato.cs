namespace refShop_DEV.Models.Restaurant
{
    public class Plato
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public int IdPromocion { get; set; }
        public float Precio { get; set; }
        public int Activo { get; set; }
        public byte[] Imagen { get; set; }
        public Categoria IdCategoriaNavigation { get; set; }
        public Promocion IdPromocionNavigation { get; set; }
    }
}
