namespace Backend.Dtos
{
    public class CreateFormular
    {
        public int IdUser { get; set; }
        public int IdOrt { get; set; }
 
        public string Startdatum { get; set; } = string.Empty;
        public string Enddatum { get; set; } = string.Empty;
 
        public string StartZeit { get; set; } = string.Empty;
        public string EndZeit { get; set; } = string.Empty;
 
        public string Status { get; set; } = "pending";
        public string GrundDerBuchung { get; set; } = string.Empty;
    }
}