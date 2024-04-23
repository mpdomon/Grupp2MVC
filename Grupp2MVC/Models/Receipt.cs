namespace Grupp2MVC.Models
{
    public class Receipt
    {
        //public string Make { get; set; }
        //public string Model { get; set; }
        //public string RegistrationNumber { get; set; }
        public int Id { get; set; }
        public double? Price { get; set; }
        public DateTime TimeOfArrival { get; set; }
        public DateTime? TimeOfDeparture { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
