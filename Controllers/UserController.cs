using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;

// Controller zur Verwaltung der Benutzer im System
namespace Backend.Controllers
{
    // Benutzer sind Mitarbeitern zugeordnet und besitzen unterschidliche Rollen
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Liefert eine Liste aller Benuzter inklusive ihrer Rollen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.Include(u => u.Rolle).ToListAsync();
        }

        // Liefert einen einzelnen Benutzer anhand der Benutzer-ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Rolle).FirstOrDefaultAsync(u => u.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // Liefert einen Benutzer anhand von Vor- und Nachnamen
        [HttpGet("byname")]
        public async Task<ActionResult<User>> GetUserByName([FromQuery] string Vorname, [FromQuery] string Nachname)
        {
            var user = await _context.Users.Include(u => u.Rolle).FirstOrDefaultAsync(u => u.Vorname == Vorname && u.Nachname == Nachname );
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // Erstellt einen neuen Benutzer im System
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.IdUser }, user);
        }

        // Aktualisiert die Daten eines bestehenden Benutzers
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.IdUser)
            {
                return BadRequest();
            }
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.IdUser == id))
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

        // Löscht einen Benutzer aus dem System
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}