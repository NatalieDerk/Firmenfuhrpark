using Backend.Db_tables;
using Backend.Dtos;
using Backend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormularController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IHubContext<BookingHub> _bookingHubContext;
        public FormularController(ApplicationDBContext context, IHubContext<BookingHub> bookingHubContext)
        {
            _context = context;
            _bookingHubContext = bookingHubContext;
        }

        // Alle Buchungen laden
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormularDto>>> GetFormular()
        {
            var formulars = await _context.Formular
                .Include(f => f.User).ThenInclude(u => u.Rolle)
                .Include(f => f.Manager).ThenInclude(m => m.Rolle)
                .Include(f => f.Fahrzeuge).ThenInclude(c => c.Standort)
                .Include(f => f.Standort)
                .Select(f => new FormularDto
                {
                    IdForm = f.IdForm,
                    User = new UserDto
                    {
                        IdUser = f.User.IdUser,
                        Vorname = f.User.Vorname,
                        Nachname = f.User.Nachname,
                        RolleName = f.User.Rolle != null ? f.User.Rolle.Name : string.Empty
                    },

                    Manager = f.Manager != null ? new UserDto
                    {
                        IdUser = f.Manager.IdUser,
                        Vorname = f.Manager.Vorname,
                        Nachname = f.Manager.Nachname,
                        RolleName = f.Manager.Rolle != null ? f.Manager.Rolle.Name : string.Empty
                    } : null,

                    Fahrzeug = f.Fahrzeuge != null ? new FahrzeugeDto
                    {
                        IdCar = f.Fahrzeuge.IdCar,
                        Marke = f.Fahrzeuge.Marke,
                        Standort = f.Fahrzeuge != null ? new StandortDto
                        {
                            IdOrt = f.Fahrzeuge.Standort.IdOrt,
                            Ort = f.Fahrzeuge.Standort.Ort
                        } : null
                    } : null,

                    Standort = f.Standort != null ? new StandortDto
                    {
                        IdOrt = f.Standort.IdOrt,
                        Ort = f.Standort.Ort
                    }
                    : new StandortDto(),

                    Status = string.IsNullOrEmpty(f.Status) ? "pending" : f.Status,
                    GrundDerBuchung = f.GrundDerBuchung ?? string.Empty,
                    StartZeit = f.StartZeit != null ? f.StartZeit.ToString() : null,
                    EndZeit = f.EndZeit != null ? f.EndZeit.ToString() : null,
                    Startdatum = f.Startdatum,
                    Enddatum = f.Enddatum,
                    Locked = f.Locked,
                    Tag = f.Tag,
                    Serienbuchung = f.Serienbuchung
                })
                .ToListAsync();

            return Ok(formulars);
        }

        // Eine Buchung nach ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FormularDto>> GetFormular(int id)
        {
            var f = await _context.Formular
                .Include(f => f.User).ThenInclude(u => u.Rolle)
                .Include(f => f.Manager).ThenInclude(m => m.Rolle)
                .Include(f => f.Fahrzeuge).ThenInclude(c => c.Standort)
                .Include(f => f.Standort)
                .FirstOrDefaultAsync(f => f.IdForm == id);

            if (f == null)
            {
                return NotFound();
            }

            var dto = new FormularDto
            {
                IdForm = f.IdForm,
                User = new UserDto
                {
                    IdUser = f.User.IdUser,
                    Vorname = f.User.Vorname,
                    Nachname = f.User.Nachname,
                    RolleName = f.User.Rolle.Name
                },

                Manager = f.Manager != null ? new UserDto
                {
                    IdUser = f.Manager.IdUser,
                    Vorname = f.Manager.Vorname,
                    Nachname = f.Manager.Nachname,
                    RolleName = f.Manager.Rolle.Name
                } : null,

                Fahrzeug = f.Fahrzeuge != null ? new FahrzeugeDto
                {
                    IdCar = f.Fahrzeuge.IdCar,
                    Marke = f.Fahrzeuge.Marke,
                    Standort = new StandortDto
                    {
                        IdOrt = f.Fahrzeuge.Standort.IdOrt,
                        Ort = f.Fahrzeuge.Standort.Ort
                    }
                } : null,

                Standort = new StandortDto
                {
                    IdOrt = f.Standort.IdOrt,
                    Ort = f.Standort.Ort
                },
                Status = f.Status,
                GrundDerBuchung = f.GrundDerBuchung,
                StartZeit = f.StartZeit?.ToString(),
                EndZeit = f.EndZeit?.ToString(),
                Startdatum = f.Startdatum,
                Enddatum = f.Enddatum
            };

            return Ok(dto);
        }


        // Neue Buchung erstellen
        [HttpPost]
        public async Task<ActionResult> PostFormular([FromBody] CreateFormular create)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                TimeSpan? startTime = null;
                TimeSpan? endTime = null;

                // StartZeit/EndZeit aus String in TimeSpam umwandeln
                if (!string.IsNullOrEmpty(create.StartZeit))
                    startTime = TimeSpan.TryParse(create.StartZeit, out var st) ? st : null;

                if (!string.IsNullOrEmpty(create.EndZeit))
                    endTime = TimeSpan.TryParse(create.EndZeit, out var et) ? et : null;

                var form = new Formular
                {
                    IdUser = create.IdUser,
                    IdOrt = create.IdOrt,
                    Startdatum = create.Startdatum,
                    Enddatum = create.Enddatum,
                    StartZeit = startTime,
                    EndZeit = endTime,
                    Status = create.Status,
                    GrundDerBuchung = create.GrundDerBuchung,
                    // Auto wird vom Manager später ausgewählt
                    IdCar = null,
                    Tag = create.Tag,
                    Serienbuchung = create.Serienbuchung
                };

                _context.Formular.Add(form);
                await _context.SaveChangesAsync();

                //SignalR - neue Buchung melden
                await _bookingHubContext.Clients.All.SendAsync("ReceiveNewBooking", form.IdForm);

                return CreatedAtAction(nameof(GetFormular), new { id = form.IdForm }, form);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }

        // Buchung aktualisieren (Manager bestätigt/storniert)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBooking update)
        {
            var form = await _context.Formular.FindAsync(id);
            if (form == null)
                return NotFound();

            // Wenn Manager Status auf "bestätigt" setzt, prüfen wir ob das Auto schon gebucht ist
            if (update.IdCar.HasValue && update.Status == "bestätigt")
            {
                var startDateTime = form.Startdatum.Date + (form.StartZeit ?? TimeSpan.Zero);
                var endDateTime = form.Enddatum.Date + (form.EndZeit ?? TimeSpan.Zero);

                // Overlap-Prüfung (Intervall überschneidet sich)
                bool overlapping = await _context.Formular
                    .Where(f =>
                        f.IdCar == update.IdCar.Value &&
                        f.IdForm != id &&
                        f.Status == "bestätigt"
                    )
                    .AnyAsync(f =>
                        (f.Startdatum.Date + (f.StartZeit ?? TimeSpan.Zero)) < endDateTime &&
                        (f.Enddatum.Date + (f.EndZeit ?? TimeSpan.Zero)) > startDateTime
                    );
                if (overlapping)
                    return BadRequest("Diese Fahrzeg ist in diesem Zeitraum bereits gebucht!");
            }

            // Felder aktualisieren
            form.IdCar = update.IdCar ?? form.IdCar;
            form.IdManager = update.IdManager ?? form.IdManager;

            if (!string.IsNullOrWhiteSpace(update.Status))
                form.Status = update.Status;

            if (update.Locked.HasValue)
                form.Locked = update.Locked.Value;

            if (update.Serienbuchung.HasValue)
                form.Serienbuchung = update.Serienbuchung.Value;

            if (!string.IsNullOrWhiteSpace(update.Tag))
                form.Tag = update.Tag;

            await _context.SaveChangesAsync();
            return Ok(form);
        }

        // Buchung löschen
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormular(int id)
        {
            var form = await _context.Formular.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            _context.Formular.Remove(form);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Alle offenen Buchungen
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Formular>>> GetPending()
        {
            var pending = await _context.Formular
            .Include(f => f.User)
            .Where(f => f.Status == null || f.Status == "" || f.Status == "pending")
            .ToListAsync();
            return Ok(pending);
        }
    }
}