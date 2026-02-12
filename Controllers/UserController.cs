using Microsoft.AspNetCore.Mvc;
using Backend.Dtos;
using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Alle Benutzer abrufen (als DTO)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            // Benutzer inkl. Rolle laden
            var users = await _context.Users
            .Include(u => u.Rolle)
            .Select(u => new UserDto
            {
                IdUser = u.IdUser,
                Vorname = u.Vorname,
                Nachname = u.Nachname,
                RolleName = u.Rolle.Name
            })
            .ToListAsync();

            return Ok(users);
        }

        // Hier wird Entity User zurückgegeben (nicht DTO)
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

        // Benutzer per Vorname und Nachname suchen
        [HttpGet("byname")]
        public async Task<ActionResult<User>> GetUserByName([FromQuery] string Vorname, [FromQuery] string Nachname)
        {
            var user = await _context.Users.Include(u => u.Rolle).FirstOrDefaultAsync(u => u.Vorname == Vorname && u.Nachname == Nachname);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // Neue Benutzer erstellen
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Benutzer in die DB hinzufügen
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.IdUser }, user);
        }

        // Benutzer aktualisieren
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            // Prüfen, ob di IDs übereinstimmen
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

        // Benuzter löschen
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Benutzer suchen
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