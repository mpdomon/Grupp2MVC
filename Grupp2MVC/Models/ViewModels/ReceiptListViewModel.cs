using System.ComponentModel.DataAnnotations;

namespace Grupp2MVC.Models.ViewModels
{
    public class ReceiptListViewModel
    {
        public int VehicleId { get; set; }
        public IEnumerable<ReceiptViewModel> Receipts { get; set; }
    }
}
