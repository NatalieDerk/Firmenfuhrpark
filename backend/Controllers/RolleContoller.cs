using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolleController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public RolleController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rolle>>> GetRolle()
        {
            return await _context.Rollen.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rolle>> GetRolle(int id)
        {
            var rolle = await _context.Rollen.FindAsync(id);
            if (rolle == null)
            {
                return NotFound();
            }
            return rolle;
        }

        [HttpPost]
        public async Task<ActionResult<Rolle>> PostUser(Rolle rolle)
        {
            _context.Rollen.Add(rolle);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRolle), new { id = rolle.IdRolle }, rolle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRolle(int id, Rolle rolle)
        {
            if (id != rolle.IdRolle)
            {
                return BadRequest();
            }
            _context.Entry(rolle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Rollen.Any(r => r.IdRolle == id))
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
        public async Task<IActionResult> DeleteRolle(int id)
        {
            var rolle = await _context.Rollen.FindAsync(id);
            if (rolle == null)
            {
                return NotFound();
            }
            _context.Rollen.Remove(rolle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<object>> GetUserWihtRole(int id)
        {
            var user = await _context.Users
            .Include(u => u.Rolle)
            .FirstOrDefaultAsync(u => u.IdUser == id);

            if (user == null)
            return NotFound();

            return new
            {
                user.IdUser,
                user.Vorname,
                user.Nachname,
                RoleId = user.IdRolle,
                RoleName = user.Rolle.Name
            };
         }

    }
}