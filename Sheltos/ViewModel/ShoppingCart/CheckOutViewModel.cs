using Sheltos.Models;
using Sheltos.ViewModel.Card;

namespace Sheltos.ViewModel.ShoppingCart
{
    public class CheckOutViewModel
    {
       
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string PaymentStatus { get; set; } = "Pending";
        public List<CheckoutItems> Items { get; set; }
        public double Price { get; set; }
        public double TotalAmount { get; set; }

    }
}
