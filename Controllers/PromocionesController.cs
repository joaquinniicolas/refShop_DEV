using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using refShop_DEV.Models.Login;
using refShop_DEV.Models.MyDbContext;
using refShop_DEV.Models.Restaurant;
using System;
using System.Security.Claims;

namespace refShop_DEV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PromocionesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public PromocionesController(MyDbContext context)
        {

            _context = context;
        }

        // GET: api/Promociones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promocion>>> GetPromociones()
        {
            return await _context.Promociones.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Promocion>> GetPromocion(int id)
        {
            var promocion = await _context.Promociones.FindAsync(id);

            if (promocion == null)
            {
                return NotFound();
            }

            return promocion;
        }



        // POST CREAR PROMOCION
        [HttpPost]

        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Promocion>> CreatePromocion(Promocion promocion)
        {
            try
            {
                // Obtener el userId del usuario autenticado
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                // Verificar si el usuario existe
                var userExists = await _context.Users.AnyAsync(u => u.Id == Convert.ToInt32(userId));
                if (!userExists)
                {
                    return BadRequest("El usuario no existe.");
                }

                // Crear la nueva promoción
                promocion.AplicadoPor = Convert.ToInt32(userId);
                _context.Promociones.Add(promocion);
                await _context.SaveChangesAsync();
                var newValue = JsonConvert.SerializeObject(promocion);
                // Registrar la operación en el log
                var log = new Log
                {
                    TableName = "Promoción",
                    RecordId = promocion.Id,
                    Operation = "Creación",
                    UserId = int.Parse(HttpContext.User.Identity.Name),
                    ColumnName = "Todas las columnas", // Reemplaza esto con el nombre de la columna que se está actualizando
                    OldValue = "-",
                    NewValue = newValue,
                    Description = "Promoción creada",
                    Timestamp = DateTime.Now
                };
                _context.Logs.Add(log);
                await _context.SaveChangesAsync();

                return Ok(promocion);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // ACTUALIZAR PROMOCION
        [HttpPut("{id}")]
        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Promocion>> UpdatePromocion(int id, Promocion promocion)
        {
            // Obtener el userId del usuario autenticado
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Verificar si el usuario existe
            var userExists = await _context.Users.AnyAsync(u => u.Id == Convert.ToInt32(userId));
            if (!userExists)
            {
                return BadRequest("El usuario no existe.");
            }

            // Buscar la promoción a actualizar
            var promocionToUpdate = await _context.Promociones.FindAsync(id);
            if (promocionToUpdate == null)
            {
                return NotFound();
            }

            // Verificar si el usuario autenticado es el creador de la promoción
            if (promocionToUpdate.AplicadoPor != Convert.ToInt32(userId))
            {
                return Unauthorized("No tiene permiso para actualizar esta promoción.");
            }
            var oldValue = JsonConvert.SerializeObject(promocion);


            // Actualizar los datos de la promoción
            promocionToUpdate.Nombre = promocion.Nombre;
            promocionToUpdate.Descuento = promocion.Descuento;
            promocionToUpdate.FechaInicio = promocion.FechaInicio;
            promocionToUpdate.FechaFin = promocion.FechaFin;
            promocionToUpdate.Estado = promocion.Estado;

            _context.Promociones.Update(promocionToUpdate);
            await _context.SaveChangesAsync();
            var newValue = JsonConvert.SerializeObject(promocion);


            // Registrar la operación en el log
            var log = new Log
            {
                TableName = "Promoción",
                RecordId = promocion.Id,
                Operation = "Actualización",
                UserId = int.Parse(HttpContext.User.Identity.Name),
                ColumnName = "Todas las columnas", // Reemplaza esto con el nombre de la columna que se está actualizando
                OldValue = oldValue,
                NewValue = newValue,
                Description = "Promoción actualizada",
                Timestamp = DateTime.Now
            };
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(promocionToUpdate);
        }


        //BORRAR PROMOCION
        [HttpDelete("{id}")]
        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Promocion>> DeletePromocion(int id)
        {
            // Obtener el userId del usuario autenticado
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Verificar si el usuario existe
            var userExists = await _context.Users.AnyAsync(u => u.Id == Convert.ToInt32(userId));
            if (!userExists)
            {
                return BadRequest("El usuario no existe.");
            }

            // Obtener la promoción a eliminar
            var promocion = await _context.Promociones.FindAsync(id);
            if (promocion == null)
            {
                return NotFound();
            }
            var oldValue = JsonConvert.SerializeObject(promocion);
            var getPromocionId = promocion.Id;
            // Eliminar la promoción de la base de datos
            _context.Promociones.Remove(promocion);
            await _context.SaveChangesAsync();
            var newValue = "--";

            // Registrar el evento en la tabla Logs
            var log = new Log
            {
                TableName = "Promoción",
                RecordId = getPromocionId,
                Operation = "Borrado",
                UserId = int.Parse(HttpContext.User.Identity.Name),
                ColumnName = "Todas las columnas", // Reemplaza esto con el nombre de la columna que se está actualizando
                OldValue = oldValue,
                NewValue = newValue,
                Description = "Promoción eliminada",
                Timestamp = DateTime.Now
            };
            await _context.SaveChangesAsync();

            return promocion;
        }



    }
}
