using System.ComponentModel;

namespace Backend.Dtos
{
    public class StandortDto
    {
        public int IdOrt { get; set; }
        public string Ort { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public int IdUser { get; set; }
        public string Vorname { get; set; } = string.Empty;
        public string Nachname { get; set; } = string.Empty;
        public string RolleName { get; set; } = string.Empty;
    }

    public class FahrzeugeDto
    {
        public int IdCar { get; set; }
        public string Marke { get; set; } = string.Empty;
        public string? Kennzeichnung { get; set; } = string.Empty;
        public StandortDto Standort { get; set; } = new StandortDto();
    }
 
    public class FormularDto
    {
        public int IdForm { get; set; }
        public UserDto User { get; set; } = new UserDto();
        public UserDto? Manager { get; set; }
        public FahrzeugeDto? Fahrzeug { get; set; }
        public StandortDto Standort { get; set; } = new StandortDto();
        public string Status { get; set; } = string.Empty;
        public string GrundDerBuchung { get; set; } = string.Empty;
        public string? StartZeit { get; set; }
        public string? EndZeit { get; set; }
        public DateTimeOffset Startdatum { get; set; }
        public DateTimeOffset Enddatum { get; set; }
        public bool Locked { get; set; }
        public string? Tag { get; set; }
        public bool Serienbuchung { get; set; }
    }
    
}