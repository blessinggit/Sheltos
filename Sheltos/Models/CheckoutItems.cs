namespace Sheltos.Models
{
    public class CheckoutItems
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }

        public int CheckoutId { get; set; }
        public Checkout Checkout { get; set; }
        public double TotalAmount { get; set; }
    }
}
