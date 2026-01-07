namespace Backend.Dtos
{
    public class UpdateBooking
    {
        public int? IdCar {get; set;}
        public int? IdManager {get; set;}
        public string Status {get; set;} = string.Empty;
    }
}