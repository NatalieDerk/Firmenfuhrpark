using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Db_tables
{
    public class Fahrzeuge
    {
        [Key]
        public int IdCar { get; set; }
        public int IdOrt { get; set; }
        public string? Marke { get; set; }
        public DateTime? DatumVonKauf { get; set; }
        public string? Farbe { get; set; }
        public string? Typ { get; set; }
        public string? Kennzeichnung { get; set; }

        public Standort? Standort { get; set; }
    }
}


