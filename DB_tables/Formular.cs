using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Db_tables
{
    public class Formular
{
    [Key]
    public int IdForm { get; set; }
    public int IdUser { get; set; }
    public int? IdManager { get; set; }
    public int? IdCar { get; set; }
    public int? IdOrt { get; set; }
    public DateTime Startdatum { get; set; }
    public DateTime Enddatum { get; set; }
    public TimeSpan? StartZeit { get; set; }
    public TimeSpan? EndZeit { get; set; }
    public string? StartZeitStr { get; set; }
    public string? EndZeitStr { get; set; }
    public string? Status { get; set; }
    public string? GrundDerBuchung { get; set; }
    public string? NameVonManager { get; set; }

    public User? User { get; set; }
    public User? Manager { get; set; }
    public Fahrzeuge? Fahrzeuge { get; set; }
    public Standort? Standort { get; set; }
}
}
