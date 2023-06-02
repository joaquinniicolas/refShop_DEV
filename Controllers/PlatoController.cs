using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using refShop_DEV.Models.Login;
using refShop_DEV.Models.MyDbContext;
using refShop_DEV.Models.Restaurant;

namespace refShop_DEV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatoController : ControllerBase
    {

        private readonly MyDbContext _context;

        public PlatoController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plato>>> GetPlatos()
        {
            var platos = await _context.Platos.ToListAsync();
            return Ok(platos);
        }

        [HttpPost]
        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Plato>> CreatePlato(Plato nuevoPlato)
        {
            _context.Platos.Add(nuevoPlato);
            await _context.SaveChangesAsync();
            var newValue = JsonConvert.SerializeObject(nuevoPlato);

            // Registro de entrada en la tabla Log
            var log = new Log
            {
                TableName = "Platos",
                RecordId = nuevoPlato.Id,
                Operation = "CREATE",
                UserId = int.Parse(HttpContext.User.Identity.Name),
                ColumnName = "-",
                OldValue = "-",
                NewValue = newValue,
                Description = "Se ha creado un nuevo plato",
                Timestamp = DateTime.UtcNow
            };
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlato), new { id = nuevoPlato.Id }, nuevoPlato);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Plato>> GetPlato(int id)
        {
            var plato = await _context.Platos.FindAsync(id);

            if (plato == null)
            {
                return NotFound();
            }

            return plato;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Plato>> UpdatePlato(int id, Plato plato)
        {
            var platoExistente = await _context.Platos.FindAsync(id);

            if (platoExistente == null)
            {
                return NotFound();
            }

            // Guardar valores antiguos para el registro en Log
            var oldValue = JsonConvert.SerializeObject(platoExistente);

            // Actualizar valores del plato existente
            platoExistente.Nombre = plato.Nombre;
            platoExistente.Descripcion = plato.Descripcion;
            platoExistente.Precio = plato.Precio;
            platoExistente.IdCategoria = plato.IdCategoria;
            platoExistente.Activo = plato.Activo;
            platoExistente.Imagen = plato.Imagen;
            platoExistente.IdPromocion = platoExistente.IdCategoria;
            _context.Entry(platoExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Guardar valores nuevos para el registro en Log
                var newValue = JsonConvert.SerializeObject(platoExistente);

                // Crear registro en Log
                var log = new Log
                {
                    TableName = "Platos",
                    RecordId = platoExistente.Id,
                    Operation = "Update",
                    UserId = int.Parse(HttpContext.User.Identity.Name),
                    ColumnName = "Todas las columnas", // Reemplaza esto con el nombre de la columna que se está actualizando
                    OldValue = oldValue,
                    NewValue = newValue,
                    Description = "Plato actualizado",
                    Timestamp = DateTime.Now
                };

                _context.Logs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await GetPlato(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlato(int id)
        {
            var plato = await _context.Platos.FindAsync(id);
            if (plato == null)
            {
                return NotFound();
            }

            _context.Platos.Remove(plato);
            await _context.SaveChangesAsync();

            return NoContent();
        }




    }
}
