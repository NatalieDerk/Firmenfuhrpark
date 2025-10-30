namespace Backend.Db_tables
{
    public class Rolle
{
    public int IdRolle { get; set; }
    public string? Name { get; set; }
    public List<User>? Users { get; set; }
}
}

