using Backend.Db_tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ApplicationDBContext _context;

        public AuthController(ApplicationDBContext context)
        {
            _context = context;
        }

        // DTO für Login
        public class LoginDto
        {
            public string Vorname { get; set; }
            public string Nachname { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            // Suche nach User anhand Vorname + Nachname
            var user = await _context.Users
            .Include(u => u.Rolle)
            .FirstOrDefaultAsync(u => u.Vorname == login.Vorname && u.Nachname == login.Nachname);

            if (user == null)
                return Unauthorized("Benutzer nicht gefunden");

            return Ok(new
            {
                user.IdUser,
                user.Vorname,
                user.Nachname,
                RoleId = user.IdRolle,
                RoleName = user.Rolle.Name
            });
        }
    }
}