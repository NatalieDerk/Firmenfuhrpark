using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fahrzeuge>>> GetFahrzeuge()
        {
            return await _context.Fahrzeuge.Include(f => f.Standort).ToListAsync();
        }

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

        [HttpPost]
        public async Task<ActionResult<Fahrzeuge>> PostFahrzeuge(Fahrzeuge car)
        {
            _context.Fahrzeuge.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFahrzeuge), new { id = car.IdCar }, car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFahrzeuge(int id, Fahrzeuge car)
        {
            if (id != car.IdCar)
            {
                return BadRequest();
            }
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