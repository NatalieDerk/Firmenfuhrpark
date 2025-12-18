using Backend.Db_tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    // Controller für die Authentifizierung von Benutzern
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

    private readonly ApplicationDBContext _context;

    // Konstruktor zur Übergabe des DbContext über Dependency Injection
    public AuthController(ApplicationDBContext context)
    {
        _context = context;
    }

    // Data Transfer Object (DOT) für Login-Daten
    public class LoginDto
    {
        public string? Vorname {get; set;}
        public string? Nachname {get; set;}
    }

    // POST-Endpunkt für die Benutzeranmeldung
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _context.Users
            .Include(u => u.Rolle)
            .FirstOrDefaultAsync(u => u.Vorname == login.Vorname && u.Nachname == login.Nachname);

            if(user == null)
            {
                return Unauthorized("Benutzer nicht gefunden");
            }
            
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