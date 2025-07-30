using Sheltos.ViewModel.Card;

namespace Sheltos.Models
{
    public class Checkout
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public List<CheckoutItems> Items { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string? PaymentMethod { get; set; }
        public double? TotalAmount { get; set; }


    }
}
