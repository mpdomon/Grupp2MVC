using System.ComponentModel.DataAnnotations;

namespace Grupp2MVC.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public VehicleType VehicleType { get; set; }
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }
        [StringLength(20)]
        public string Make { get; set; }
        [StringLength(20)]
        public string Model { get; set; }
        [Range(0,10)]
        public int AmountOfWheels { get; set; }
        public DateTime TimeOfArrival { get; set; }
        public DateTime? TimeOfDeparture { get; set; }
    }
}
