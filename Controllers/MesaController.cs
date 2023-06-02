using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using refShop_DEV.Models.Login;
using refShop_DEV.Models.MyDbContext;
using refShop_DEV.Models.Restaurant;
using System.Data;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace refShop_DEV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MesaController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;


        public MesaController(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        

        



        //[Authorize(Roles = "4")]
        //[Authorize(Roles = "3")]
        //[Authorize(Roles = "2")]
        [HttpGet("getMesas")]

        public async Task<ActionResult<IEnumerable<MesaDTO>>> GetMesas()
        {
            var mesas = await _context.Mesa.Include(m => m.IdMozoNavigation)
                .Include(m => m.CreadoPorNavigation)
                .Include(m => m.ModificadoPorNavigation)
                .Select(m=> new MesaDTO {
                    Id = m.Id,
                    Numero = m.Numero,
                    Capacidad = m.Capacidad,
                    Estado = m.Estado,
                    CreadoEl = m.CreadoEl,
                    CreadoPor = m.CreadoPor,
                    ModificadoEl = m.ModificadoEl,
                    ModificadoPor = m.ModificadoPor,
                    IdUsuario = m.IdUsuario,
                    IdMozo = m.IdMozo,
                    Creador = m.CreadoPorNavigation.FirstName + ' ' + m.CreadoPorNavigation.LastName,
                    Modificador = m.ModificadoPorNavigation.FirstName + ' ' + m.CreadoPorNavigation.LastName,
                    MozoEncargado = m.IdMozoNavigation.FirstName + ' ' + m.IdMozoNavigation.LastName

            }).ToListAsync();
            //var mesasDto = _mapper.Map<List<MesaDTO>>(mesas);
            return mesas;

        }



        [HttpGet("{id}")]
        [Authorize(Roles = "4")]
        [Authorize(Roles = "3")]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<Mesa>> GetMesa(int id)
        {
            var mesa = await _context.Mesa.FindAsync(id);

            if (mesa == null)
            {
                return NotFound();
            }

            return mesa;
        }

        //[Authorize(Roles = "4")]
        //[Authorize(Roles = "3")]
        //[Authorize(Roles = "2")]
        [HttpPost("createMesa")]
        public async Task<ActionResult<Mesa>> CreateMesa(Mesa nuevaMesa)
        {
            _context.Mesa.Add(nuevaMesa);
            var newValue = nuevaMesa.Id;
            // Registro de entrada en la tabla Log
            var log = new Log
            {
                TableName = "Mesas",
                RecordId = nuevaMesa.Id,
                Operation = "CREATE",
                UserId = GetUserId(),
                ColumnName = "-",
                OldValue = "-",
                NewValue = Convert.ToString(newValue),
                Description = "Se ha creado una nueva mesa",
                Timestamp = DateTime.UtcNow
            };
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMesa), new { id = nuevaMesa.Id }, nuevaMesa);
        }

        //[HttpPut("{id}")]
        //[Authorize(Roles = "4")]
        //[Authorize(Roles = "3")]
        //[Authorize(Roles = "2")]
        //public async Task<ActionResult<Mesa>> UpdateMesa(int id, Mesa mesaActualizada)
        //{
        //    if (id != mesaActualizada.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var mesa = await _context.Mesa.FindAsync(id);
        //    if (mesa == null)
        //    {
        //        return NotFound();
        //    }

        //    var oldValue = JsonConvert.SerializeObject(mesa);

        //    mesa.Numero = mesaActualizada.Numero;
        //    mesa.Capacidad = mesaActualizada.Capacidad;
        //    mesa.Estado = mesaActualizada.Estado;
        //    mesa.IdMozo = mesa.IdMozo;
        //    mesa.IdUsuario = mesa.IdUsuario;
        //    mesa.ModificadoPor = mesa.ModificadoPor;


        //    try
        //    {
        //        await _context.SaveChangesAsync();

        //        var newValue = JsonConvert.SerializeObject(mesa);

        //        // Registro de entrada en la tabla Log
        //        var log = new Log
        //        {
        //            TableName = "Mesas",
        //            RecordId = mesa.Id,
        //            Operation = "UPDATE",
        //            UserId = int.Parse(HttpContext.User.Identity.Name),
        //            ColumnName = "-",
        //            OldValue = oldValue,
        //            NewValue = newValue,
        //            Description = "Se ha actualizado una mesa",
        //            Timestamp = DateTime.UtcNow
        //        };
        //        _context.Logs.Add(log);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (await GetMesa(id) == null)
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //[Authorize(Roles = "4")]
        //[Authorize(Roles = "3")]
        //[Authorize(Roles = "2")]

        //public async Task<IActionResult> DeleteMesa(int id)
        //{
        //    var mesa = await _context.Mesa.FindAsync(id);
        //    if (mesa == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Mesa.Remove(mesa);
        //    await _context.SaveChangesAsync();

        //    // Registro de entrada en la tabla Log
        //    var log = new Log
        //    {
        //        TableName = "Mesas",
        //        RecordId = id,
        //        Operation = "DELETE",
        //        UserId = int.Parse(HttpContext.User.Identity.Name),
        //        ColumnName = "-",
        //        OldValue = JsonConvert.SerializeObject(mesa),
        //        NewValue = "-",
        //        Description = $"Se ha eliminado la mesa con ID {id}",
        //        Timestamp = DateTime.UtcNow
        //    };
        //    _context.Logs.Add(log);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


    }
}



//////////////////////PARA GET MESAS

//var options = new JsonSerializerOptions()
//{
//    ReferenceHandler = ReferenceHandler.Preserve,

//};

//var mesas = await _context.Mesa
//    .Include(m => m.CreadoPorNavigation)
//    .Include(m => m.ModificadoPorNavigation)
//    .Include(m => m.IdUsuarioNavigation)
//    .Include(m => m.IdMozoNavigation)
//    .ToListAsync();

////var mesasDto = _mapper.Map<List<MesaDTO>>(mesas);

//return new JsonResult(mesas, options);

//var mesas = await _context.Mesa
//   .Select(m => new MesaDTO
//   {
//       Id = m.Id,
//       Numero = m.Numero,
//       Capacidad = m.Capacidad,
//       Estado = m.Estado,
//       CreadoEl = m.CreadoEl,
//       CreadoPor = m.CreadoPor,
//       ModificadoEl = m.ModificadoEl,
//       ModificadoPor = m.ModificadoPor,
//       IdUsuario = m.IdUsuario,
//       IdMozo = m.IdMozo,
//       Creador = m.CreadoPorNavigation.FirstName + ' '+ m.CreadoPorNavigation.LastName,
//       Modificador = m.ModificadoPorNavigation.FirstName + ' '+ m.ModificadoPorNavigation.LastName,
//       MozoEncargado = m.IdMozoNavigation.FirstName + ' ' + m.IdMozoNavigation.LastName,
//   })
//   .ToListAsync();