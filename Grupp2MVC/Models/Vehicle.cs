using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grupp2MVC.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        [DisplayName("Type of vehicle")]
        public VehicleType VehicleType { get; set; }
        [DisplayName("Registration Number")]
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }
        [StringLength(20)]
        public string Make { get; set; }
        [StringLength(20)]
        public string Model { get; set; }
        [Range(0,10)]
        [DisplayName("Number of wheels")]
        public int AmountOfWheels { get; set; }
        [DisplayName("Arrival")]
        public DateTime TimeOfArrival { get; set; }
        [DisplayName("Departure")]
        public DateTime? TimeOfDeparture { get; set; }
    }
}
