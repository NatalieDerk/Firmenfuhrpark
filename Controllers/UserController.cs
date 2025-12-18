using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UserController: ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
            .Include(u => u.Rolle)
            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUsers(int id)
        {
            var user = await _context.Users
            .Include(u => u.Rolle)
            .ToListAsync();

            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var rolle = await _context.Rollen.FindAsync(user.IdRolle);
            if(rolle == null)
                return BadRequest("Rolle existiert nicht");

            user.Rolle = rolle;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsers), new {id = user.IdUser}, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.IdUser)
            {
                return BadRequest();
            }
            _context.Entry(user).State = EntityState.Modified;

            return NoContent();
        }

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