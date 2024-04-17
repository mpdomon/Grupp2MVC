namespace Grupp2MVC.Models
{
    public class ParkedVehicle
    {
        public string RegistrationNumber { get; }
        public Vehicle Vehicle { get; set; }
        public DateTime TimeOfArrival { get; }

        public ParkedVehicle(Vehicle vehicle, DateTime timeOfArrival )
        {
            Vehicle = vehicle;
            RegistrationNumber = vehicle.RegistrationNumber;
            TimeOfArrival = timeOfArrival;
        }
    }
}
