using System.ComponentModel.DataAnnotations;

namespace Backend.Db_tables
{
    public class Standort
{
    [Key]
    public int IdOrt { get; set; }
    public string? Ort { get; set; }
    public List<Fahrzeuge>? Fahrzeuge { get; set; }
}
}
