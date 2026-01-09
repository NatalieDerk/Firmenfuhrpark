using Backend.Db_tables;
using Backend.Dtos;
using Backend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormularController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IHubContext<BookingHub> _bookingHubContext;
        public FormularController(ApplicationDBContext context, IHubContext<BookingHub> bookingHubContext)
        {
            _context = context;
            _bookingHubContext = bookingHubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Formular>>> GetFormular()
        {
            return await _context.Formular
            .Include(f => f.User)
            .Include(f => f.Manager)
            .Include(f => f.Fahrzeuge)
            .Include(f => f.Standort)
            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Formular>> GetFormular(int id)
        {
            var formular = await _context.Formular
            .Include(f => f.User)
            .Include(f => f.Manager)
            .Include(f => f.Fahrzeuge)
            .Include(f => f.Standort)
            .FirstOrDefaultAsync(f => f.IdForm == id);

            if (formular == null)
            {
                return NotFound();
            }
            return Ok(formular);
        }

        [HttpPost]
        public async Task<ActionResult> PostFormular([FromBody] CreateFormular create)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
 
            try
            {
            TimeSpan? startTime = null;
            TimeSpan? endTime = null;
    
            if (!string.IsNullOrEmpty(create.StartZeit))
                startTime = TimeSpan.TryParse(create.StartZeit, out var st) ? st : null;
    
            if (!string.IsNullOrEmpty(create.EndZeit))
                endTime = TimeSpan.TryParse(create.EndZeit, out var et) ? et : null;
    
            var form = new Formular
            {
                IdUser = create.IdUser,
                IdOrt = create.IdOrt,
                Startdatum = create.Startdatum,
                Enddatum = create.Enddatum,
                StartZeit = startTime,
                EndZeit = endTime,
                Status = create.Status,
                GrundDerBuchung = create.GrundDerBuchung,
                IdCar = null
            };
    
            _context.Formular.Add(form);
            await _context.SaveChangesAsync();
    
            return CreatedAtAction(nameof(GetFormular), new { id = form.IdForm }, form);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
}

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormular(int id, Formular form)
        {
            if (id != form.IdForm)
            {
                return BadRequest();
            }
            _context.Entry(form).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Formular.Any(f => f.IdForm == id))
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
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormular(int id)
        {
            var form = await _context.Formular.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            _context.Formular.Remove(form);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Formular>>> GetPending()
        {
            var pending = await _context.Formular
            .Include(f => f.User)
            .Where(f => f.Status == null || f.Status == "" || f.Status == "pending")
            .ToListAsync();
            return Ok(pending);
        }

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveForm(
            int id,
            [FromQuery] int idCar,
            [FromQuery] int idManager,
            [FromQuery] string status
        )
        {
           var form = await _context.Formular.FindAsync(id);
           if (form == null)
                return NotFound();

            form.IdCar = idCar;
            form.IdManager = idManager;
            form.Status = status;

            await _context.SaveChangesAsync();
            return Ok(form);
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectForm(
            int id,
            [FromQuery] int idManager
        )
        {
            var form = await _context.Formular.FindAsync(id);
            if (form == null)
                return NotFound();

            form.Status = "stornieren";
            form.IdManager = idManager;

            await _context.SaveChangesAsync();
            return Ok(form);
        }
    }
}