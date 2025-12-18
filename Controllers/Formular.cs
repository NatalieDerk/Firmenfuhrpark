using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;

namespace Backend.Controllers
{
    // Controller zur Verwaltung der Buchungsformulare
    [Route("api/[controller]")]
    [ApiController]
    public class FormularController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public FormularController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Gibt alle Buchungen inklusive zugehöriger Benutzer-, Manager-, Fahrzeug- und Standortinformationen zurück
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

        // Gibt eine einzelne Buchung anhand der Formular-ID zurück
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

        // Erstellt eine neue Fahrzeugbuchung
        [HttpPost]
        public async Task<ActionResult<Formular>> PostFormular(Formular form)
        {
            _context.Formular.Add(form);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFormular), new { id = form.IdForm }, form);
        }

        // Aktualisiert eine bestehende Buchung
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
        
        // Löscht eine Buchung aus der Datenbank
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

        // Gibt alle offenen bzw. noch nicht bearbeiteten Buchungen zurück
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Formular>>> GetPending()
        {
            var pending = await _context.Formular
            .Include(f => f.User)
            .Where(f => f.Status == null || f.Status == "" || f.Status == "pending")
            .ToListAsync();
            return Ok(pending);
        }

        // Genehmigt eine Buchung durch den Manager und weist ein Fahrzeug zu
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

        // Lehnt eine Buchung ab und setzt den Status auf "storniert"
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