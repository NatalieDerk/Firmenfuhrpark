namespace Backend.Db_tables
{
    public class User
{
    public int IdUser { get; set; }
    public int IdRolle { get; set; }
    public string? Vorname { get; set; }
    public string? Nachname { get; set; }

    public Rolle Rolle { get; set; }
}
}