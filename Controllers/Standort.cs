using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetStandort()
        {
            var standort = await _context.Standorte
                .Select(s => new {s.IdOrt, s.Ort})
                .ToListAsync();
            return Ok(standort);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Standort>> GetStandort(int id)
        {
            var ort = await _context.Standorte.FindAsync(id);
            if (ort == null)
            {
                return NotFound();
            }
            return ort;
        }

        [HttpPost]
        public async Task<ActionResult<Standort>> PostStandort(Standort ort)
        {
            _context.Standorte.Add(ort);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStandort), new { id = ort.IdOrt }, ort);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStandort(int id, Standort ort)
        {
            if (id != ort.IdOrt)
            {
                return BadRequest();
            }
            _context.Entry(ort).State = EntityState.Modified;

            try
            {
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
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStandort(int id)
        {
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