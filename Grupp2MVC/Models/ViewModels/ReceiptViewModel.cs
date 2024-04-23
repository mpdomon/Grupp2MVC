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
        [Display(Name="From")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd mm:ss}")]
        public DateTime TimeOfArrival { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name="To")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd mm:ss}")]
        public DateTime? TimeOfDeparture { get; set; }
    }
}
