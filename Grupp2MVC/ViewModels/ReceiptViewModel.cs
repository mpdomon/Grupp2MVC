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

        public double CalculateParkingPrice(DateTime timeOfArrival, DateTime timeOfDeparture)
        {
            double hourlyRate = 0.75;

            var timeDifference = timeOfDeparture - timeOfArrival;
            return hourlyRate * Math.Round((double)timeDifference.TotalSeconds / 3600, 0);
        }
    }
}
