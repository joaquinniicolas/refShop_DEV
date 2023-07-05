using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using refShop_DEV.Models.Login;
using refShop_DEV.Models.MyDbContext;
using refShop_DEV.Models.Restaurant;
using refShop_DEV.Services;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace refShop_DEV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriasController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CategoriasController(MyDbContext context) 
        {

            _context = context;

        }

        private int GetUserId()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);
            }
            throw new Exception("User ID not found.");


        }

        [HttpGet("getCategorias")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            var categorias = await _context.Categorias
                .Select(c => new CategoriaDTO
                {
                    Id = c.id,
                    Nombre = c.nombre,
                    Descripcion = c.descripcion,
                    CreadoEl = c.creado_el,
                    CreadoPor = c.IdUsuarioNavigation.Id,
                    MostrarEnSitio = c.mostrar_en_sitio,
                    UsuarioNombre = c.IdUsuarioNavigation.FirstName,
                    UsuarioApellido = c.IdUsuarioNavigation.LastName
                })
                .ToListAsync();

            return Ok(categorias);
        }

        [HttpGet("getCategoria/{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias
                .Where(c => c.id == id)
                .Select(c => new CategoriaDTO
                {
                    Id = c.id,
                    Nombre = c.nombre,
                    Descripcion = c.descripcion,
                    CreadoEl = c.creado_el,
                    CreadoPor = c.IdUsuarioNavigation.Id,
                    MostrarEnSitio = c.mostrar_en_sitio,
                    
                    UsuarioNombre = c.IdUsuarioNavigation.FirstName,
                    UsuarioApellido = c.IdUsuarioNavigation.LastName
                })
                .FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }




        //[Authorize(Roles = "4")]
        //[Authorize(Roles = "3")]
        //[Authorize(Roles = "2")]
        [HttpPost("createCategoria")]

        public async Task<ActionResult<Categoria>> CreateCategoria(Categoria categoria)
        {
            categoria.creado_el = DateTime.Now;
            categoria.creado_por = GetUserId();
            if(categoria.mostrar_en_sitio == null)
            {
                categoria.mostrar_en_sitio = 0;
            }
            
            _context.Categorias.Add(categoria);
            var newValue = JsonConvert.SerializeObject(categoria);

            // Registro de entrada en la tabla Log
            var log = new Log
            {
                TableName = "Categorias",
                RecordId = categoria.id,
                Operation = "CREATE",
                UserId = GetUserId(),
                ColumnName = "-",
                OldValue = "-",
                NewValue = newValue,
                Description = "Se ha creado una nueva categoría",
                Timestamp = DateTime.UtcNow
            };
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.id }, categoria);
        }


        //[Authorize(Roles = "4")]
        //[Authorize(Roles = "3")]
        //[Authorize(Roles = "2")]
        [HttpPut("updateCategoria/{id}")]
        public async Task<ActionResult<CategoriaDTO>> UpdateCategoria(int id, Categoria categoria)
        {

            var categoriaExistente = await _context.Categorias.FindAsync(id);


            

            if (categoriaExistente == null)
            {
                return NotFound();
            }
            categoriaExistente.nombre = categoria.nombre;
            categoriaExistente.descripcion = categoria.descripcion;
            categoriaExistente.mostrar_en_sitio = categoria.mostrar_en_sitio;

            // Guardar valores antiguos para el registro en Log
            var oldValue = JsonConvert.SerializeObject(categoriaExistente);

            // Actualizar valores de la categoría existente
            categoriaExistente.nombre = categoria.nombre;
            categoriaExistente.descripcion = categoria.descripcion;
            categoriaExistente.mostrar_en_sitio = categoria.mostrar_en_sitio;

            var newValue = JsonConvert.SerializeObject(categoriaExistente);

            

            _context.Entry(categoriaExistente).State = EntityState.Modified;

            var categoriaExistenteWithUser = await _context.Categorias
                   .Include(c => c.IdUsuarioNavigation) // Carga explícita de la propiedad IdUsuarioNavigation
                   .FirstOrDefaultAsync(c => c.id == id);

            var categoriaDTO = new CategoriaDTO
            {
                Id = categoriaExistente.id,
                Nombre = categoriaExistente.nombre,
                Descripcion = categoriaExistente.descripcion,
                CreadoEl = categoriaExistente.creado_el,
                CreadoPor = categoriaExistenteWithUser.IdUsuarioNavigation.Id,
                MostrarEnSitio = categoriaExistenteWithUser.mostrar_en_sitio,
                UsuarioNombre = categoriaExistenteWithUser.IdUsuarioNavigation.FirstName + categoriaExistenteWithUser.IdUsuarioNavigation.LastName
            };

            try
            {

                // Guardar valores nuevos para el registro en Log

                // Crear registro en Log
                var log = new Log
                {
                    TableName = "Categorias",
                    RecordId = categoriaExistente.id,
                    Operation = "Update",
                    UserId = GetUserId(),
                    ColumnName = "Todas las columnas", // Reemplaza esto con el nombre de la columna que se está actualizando
                    OldValue = oldValue,
                    NewValue = newValue,
                    Description = "Categoría actualizada",
                    Timestamp = DateTime.Now
                };

                _context.Logs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

           





            return categoriaDTO;
        }


        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(c => c.id == id);
        }

        //[Authorize(Roles = "4")]
        //[Authorize(Roles = "3")]
        //[Authorize(Roles = "2")]
        [HttpDelete("deleteCategoria/{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            var getCategoriaId = categoria.id;
            // Guardar el estado anterior de la categoria para el registro en el Log
            var oldValue = JsonConvert.SerializeObject(categoria);

            _context.Categorias.Remove(categoria);

            // Agregar registro al Log
            var log = new Log
            {
                TableName = "Categorias",
                RecordId = getCategoriaId,
                Operation = "Delete",
                UserId = GetUserId(),
                ColumnName = "Todas las columnas", // Reemplaza esto con el nombre de la columna que se está actualizando
                OldValue = oldValue,
                NewValue = "-",
                Description = "Categoría eliminada",
                Timestamp = DateTime.Now
            };
            _context.Logs.Add(log);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        

       






    }
}
