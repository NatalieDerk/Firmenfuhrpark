using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormularController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public FormularController(ApplicationDBContext context)
        {
            _context = context;
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
        public async Task<ActionResult<Formular>> PostFormular(Formular form)
        {
            _context.Formular.Add(form);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFormular), new { id = form.IdForm }, form);
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