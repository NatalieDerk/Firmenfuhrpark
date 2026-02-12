using Microsoft.AspNetCore.Mvc;
using Backend.Dtos;
using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandortController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StandortController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Alle Standorte abrufen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetStandort()
        {
            var standorte = await _context.Standorte
            .Select(s => new { s.IdOrt, s.Ort })
            .ToListAsync();

            return Ok(standorte);
        }

        // Einen Standort per ID abrufen
        [HttpGet("{id}")]
        public async Task<ActionResult<StandortDto>> GetStandort(int id)
        {
            // Wir suchen nach IdOrt und mappen direkt in ein DTO
            var ort = await _context.Standorte
                .Where(o => o.IdOrt == id)
                .Select(o => new StandortDto
                {
                    IdOrt = o.IdOrt,
                    Ort = o.Ort
                })
                .FirstOrDefaultAsync();

            if (ort == null)
            {
                return NotFound();
            }
            return ort;
        }

        // Neue Standort erstellen 
        [HttpPost]
        public async Task<ActionResult<StandortDto>> PostStandort(Standort ort)
        {
            // Standort in die Datenbnk hinzufügen
            _context.Standorte.Add(ort);
            await _context.SaveChangesAsync();

            // Ich gebe DTO zurück, damit das Fronten keine Entity direkt bekommt
            var dto = new StandortDto
            {
                IdOrt = ort.IdOrt,
                Ort = ort.Ort
            };

            return CreatedAtAction(nameof(GetStandort), new { id = ort.IdOrt }, dto);
        }

        // Standort aktualisieren
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStandort(int id, Standort ort)
        {
            // Prüfen, ob die ID in der URL mit der Objekt-ID übereinstimmt
            if (id != ort.IdOrt)
            {
                return BadRequest();
            }
            // Objekt als "Modified" markieren
            _context.Entry(ort).State = EntityState.Modified;

            try
            {
                // Änderungen speichern
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Standorte.Any(o => o.IdOrt == id))
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

        // Standort löschen
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStandort(int id)
        {
            // Standort in der DB suchen
            var ort = await _context.Standorte.FindAsync(id);
            if (ort == null)
            {
                return NotFound();
            }
            _context.Standorte.Remove(ort);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}