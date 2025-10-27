namespace Backend.Db_tables
{
    public interface Fahrzeuge
    {
        public int IdCar { get; set; }
        public int IdOrt { get; set; }
        public string Marke { get; set; }
        public DateTime? DatumVonKauf { get; set; }
        public string Farbe { get; set; }
        public string Typ { get; set; }
        public string Kennzeichung { get; set; }

        public Standort Standort { get; set; }
    }
}


