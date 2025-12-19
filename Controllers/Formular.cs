using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;
using Backend.Dtos;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
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
        public async Task<ActionResult<IEnumerable<object>>> GetFormular()
        {
            var data = await _context.Formular
            .Include(f => f.User)
            .Include(f => f.Manager)
            .Include(f => f.Fahrzeuge)
            .Include(f => f.Standort)
            .ToListAsync();

            var result = data.Select(f => new
            {
                f.IdForm,
                f.IdUser,
                f.IdManager,
                f.IdCar,
                f.IdOrt,
                startdatum = f.Startdatum.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                enddatum = f.Enddatum.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                startZeit = f.StartZeit.HasValue ? f.StartZeit.Value.ToString(@"hh\:mm\:ss") : null,
                endZeit = f.EndZeit.HasValue ? f.EndZeit.Value.ToString(@"hh\:mm\:ss") : null,
                f.Status,
                f.GrundDerBuchung,
                user = f.User !=null ? new {f.User.Vorname, f.User.Nachname} : null,
                manager = f.Manager !=null ? new {f.Manager.Vorname, f.Manager.Nachname} : null,
                fahrzeuge = f.Fahrzeuge !=null ? new {f.Fahrzeuge.Marke, f.Fahrzeuge.Kennzeichnung} : null,
                standort = f.Standort !=null ? new {f.Standort.IdOrt, f.Standort.Ort} : null
            });

            return Ok(result);
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
        public async Task<ActionResult> PostFormular([FromBody] CreateFormular create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TimeSpan? startTime = null;
            TimeSpan? endTime = null;

            if (!string.IsNullOrEmpty(create.StartZeit))
            {
                startTime = TimeSpan.TryParse(create.StartZeit, out var st) ? st : null;
            }

            if (!string.IsNullOrEmpty(create.EndZeit))
            {
                endTime = TimeSpan.TryParse(create.EndZeit, out var st) ? st : null;
            }

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

            return Ok(form);
        }

        // Aktualisiert einen existierenden Formulareintrag. Prüft, ob die Id übereinstimmt
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBooking update)
        {
           var form = await _context.Formular.FindAsync(id);
           if (form == null)
            {
                return NotFound();
            }

            if (update.IdCar.HasValue)
            {
                form.IdCar = update.IdCar;
            }

            if (update.IdManager.HasValue)
            {
                form.IdManager = update.IdManager;
            }

            form.Status = update.Status;

            await _context.SaveChangesAsync();
            return Ok(form);
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

            return Ok();
        }
    }
}