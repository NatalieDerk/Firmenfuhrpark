using Microsoft.AspNetCore.Mvc;
using Backend.Dtos;
using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FahrzeugeController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public FahrzeugeController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Gibt alle Fahrzeuge zurück
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FahrzeugeDto>>> GetFahrzeuge()
        {
            var cars = await _context.Fahrzeuge
            .Include(f => f.Standort)
            .Select(f => new FahrzeugeDto
            {
                IdCar = f.IdCar,
                Marke = f.Marke,
                Kennzeichnung = f.Kennzeichnung ?? "",

                // Standort kann theoretisch null sein
                Standort = new StandortDto
                {
                    IdOrt = f.Standort.IdOrt,
                    Ort = f.Standort.Ort
                }
            })
            .ToListAsync();

            return Ok(cars);
        }

        // Gibt ein einzelnes Fahrzeug nach ID zurück
        [HttpGet("{id}")]
        public async Task<ActionResult<Fahrzeuge>> GetFahrzeuge(int id)
        {
            var car = await _context.Fahrzeuge.Include(f => f.Standort).FirstOrDefaultAsync(f => f.IdCar == id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        //Erstellt ein neues Fahrzeug
        [HttpPost]
        public async Task<ActionResult<Fahrzeuge>> PostFahrzeuge(Fahrzeuge car)
        {
            _context.Fahrzeuge.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFahrzeuge), new { id = car.IdCar }, car);
        }

        //Aktualisiert ein Fahrzeug
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFahrzeuge(int id, Fahrzeuge car)
        {
            if (id != car.IdCar)
            {
                return BadRequest();
            }
            // Hier wird der komplette Entity überschrieben
            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Fahrzeuge.Any(f => f.IdCar == id))
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

        // Löscht ein Fahrzeug
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFahrzeuge(int id)
        {
            var car = await _context.Fahrzeuge.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            _context.Fahrzeuge.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}