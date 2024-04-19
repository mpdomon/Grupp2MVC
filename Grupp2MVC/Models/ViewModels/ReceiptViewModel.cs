using System.ComponentModel.DataAnnotations;

namespace Grupp2MVC.Models.ViewModels
{
    public class ReceiptViewModel
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        [Display(Name="Paid")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Display(Name="To")]
        [DataType(DataType.DateTime)]
        public DateTime TimeOfArrival { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name="From")]
        public DateTime TimeOfDeparture { get; set; }
    }
}
