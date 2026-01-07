using System.ComponentModel.DataAnnotations;

namespace Backend.Db_tables
{
    public class User
{
    [Key]
    public int IdUser { get; set; }
    public int IdRolle { get; set; }
    public string? Vorname { get; set; } = string.Empty;
    public string? Nachname { get; set; } = string.Empty;

    public Rolle? Rolle { get; set; }
}
}