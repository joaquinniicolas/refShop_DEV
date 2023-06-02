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
    public class PedidoController : ControllerBase
    {
        private readonly MyDbContext _context;

        public PedidoController(MyDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos.ToListAsync();
            return Ok(pedidos);
        }

        [HttpPost]
        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Pedido>> CrearPedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            var newValue = JsonConvert.SerializeObject(pedido);

            // Registro de entrada en la tabla Log
            var log = new Log
            {
                TableName = "Pedidos",
                RecordId = pedido.Id,
                Operation = "CREATE",
                UserId = int.Parse(HttpContext.User.Identity.Name),
                ColumnName = "-",
                OldValue = "-",
                NewValue = newValue,
                Description = "Se ha creado un nuevo pedido",
                Timestamp = DateTime.UtcNow
            };
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Pedido>> UpdatePedido(int id, Pedido pedido)
        {
            var pedidoExistente = await _context.Pedidos.FindAsync(id);

            if (pedidoExistente == null)
            {
                return NotFound();
            }

            // Guardar valores antiguos para el registro en Log
            var oldValue = JsonConvert.SerializeObject(pedidoExistente);

            // Actualizar valores de la categoría existente
            pedidoExistente.IdPlato = pedido.IdPlato;
            pedidoExistente.IdMesa = pedido.IdMesa;
            pedidoExistente.EstadoPedido = pedido.EstadoPedido;
            pedidoExistente.Observaciones = pedido.Observaciones;
            pedidoExistente.FechaPedido = pedido.FechaPedido;
            pedidoExistente.IdUsuario = pedido.IdUsuario;
            pedidoExistente.Total = pedidoExistente.Total;
            

            _context.Entry(pedidoExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Guardar valores nuevos para el registro en Log
                var newValue = JsonConvert.SerializeObject(pedidoExistente);

                // Crear registro en Log
                var log = new Log
                {
                    TableName = "Pedidos",
                    RecordId = pedidoExistente.Id,
                    Operation = "Update",
                    UserId = int.Parse(HttpContext.User.Identity.Name),
                    ColumnName = "Todas las columnas", // Reemplaza esto con el nombre de la columna que se está actualizando
                    OldValue = oldValue,
                    NewValue = newValue,
                    Description = "Pedido actualizado",
                    Timestamp = DateTime.Now
                };

                _context.Logs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
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
        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(c => c.Id == id);
        }

        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            var getPedidoId = pedido.Id;
            // Guardar el estado anterior de el pedido para el registro en el Log
            var oldValue = JsonConvert.SerializeObject(pedido);

            _context.Pedidos.Remove(pedido);

            // Agregar registro al Log
            var log = new Log
            {
                TableName = "Pedidos",
                RecordId = getPedidoId,
                Operation = "Delete",
                UserId = int.Parse(HttpContext.User.Identity.Name),
                ColumnName = "Todas las columnas", // Reemplaza esto con el nombre de la columna que se está actualizando
                OldValue = oldValue,
                NewValue = "-",
                Description = "Pedido eliminado",
                Timestamp = DateTime.Now
            };
            _context.Logs.Add(log);

            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
