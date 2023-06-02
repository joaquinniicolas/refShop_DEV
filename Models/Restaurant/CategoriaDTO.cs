namespace refShop_DEV.Models.Restaurant
{
    public class CategoriaDTO
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? CreadoEl { get; set; }
        public int? CreadoPor { get; set; }
        public int? MostrarEnSitio { get; set; }

        
        public string? UsuarioNombre { get; set; }

        public string? UsuarioApellido { get; set; }
    }
}
