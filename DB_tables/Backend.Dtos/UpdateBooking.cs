using System.ComponentModel;
using System.Diagnostics;

namespace Backend.Dtos
{
    public class UpdateBooking
    {
        public int? IdCar {get; set;}
        public int? IdManager {get; set;}
        public string Status {get; set;}
        public bool? Locked {get; set; }
        public bool? Serienbuchung {get; set; }
        public string? Tag {get; set; }
    }
}