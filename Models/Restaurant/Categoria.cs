using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using refShop_DEV.Models.Login;
using refShop_DEV.Services;
using refShop_DEV.Services.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace refShop_DEV.Models.Restaurant
{
    [Table("Categoria")]
    public class Categoria
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public DateTime? creado_el { get; set; }
        public int? creado_por { get; set; }
        public int? mostrar_en_sitio { get; set; }
        [ForeignKey("creado_por")]
        public virtual User? IdUsuarioNavigation { get; set; }



    }
}
