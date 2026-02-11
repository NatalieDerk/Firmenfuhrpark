namespace Backend.Dtos
{
    public class CreateFormular
    {
        public int IdUser { get; set; }
        public int IdOrt { get; set; }
 
        public DateTimeOffset Startdatum { get; set; }
        public DateTimeOffset Enddatum { get; set; }
 
        public string StartZeit { get; set; } = string.Empty;
        public string EndZeit { get; set; } = string.Empty;
 
        public string Status { get; set; } = "pending";
        public string GrundDerBuchung { get; set; } = string.Empty;
        public string? Tag { get; set;}
        public bool Serienbuchung { get; set; } = false;
    }
}