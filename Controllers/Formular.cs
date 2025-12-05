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

        // Konstrurtor: Datenbankkontext wird über Dependecy Injection bereitgestellt
        public FormularController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Liefert alle Formular-Einträge inklusive verbundere Tabellen
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

        // Liefert ein einzeles Formular-Objekt anhand der Id mit allen Navigationseingenschaften
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

        // Erstellt einen neuen Formulareintrag und Konvertiert Zeitangaben (string) in TimeSpan
        [HttpPost]
        public async Task<ActionResult<Formular>> PostFormular(Formular form)
        {
            try
            {
                form.Startdatum = DateTime.SpecifyKind(form.Startdatum, DateTimeKind.Utc);
                form.Enddatum = DateTime.SpecifyKind(form.Enddatum, DateTimeKind.Utc);


                if (form.StartZeitStr.HasValue)
                    form.StartZeit = TimeSpan.Parse(form.StartZeit.ToString());

                if (form.EndZeitStr.HasValue)
                    form.EndZeit = TimeSpan.Parse(form.EndZeit.ToString());
           
            Console.WriteLine("Received form:" + Newtonsoft.Json.JsonConvert.SerializeObject(form));
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

        // Aktualisiert einen existierenden Formulareintrag. Prüft, ob die Id übereinstimmt
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
        
        // Löscht ein Formular anhand der Id
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
    }
}