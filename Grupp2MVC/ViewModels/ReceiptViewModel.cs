namespace Grupp2MVC.ViewModels
{
    public class ReceiptViewModel
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public double Price { get; set; }
        public DateTime TimeOfArrival { get; set; }
        public DateTime TimeOfDeparture { get; set; }
    }
}
