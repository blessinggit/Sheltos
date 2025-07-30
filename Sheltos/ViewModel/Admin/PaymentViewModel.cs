namespace Sheltos.ViewModel
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } 
        public List<string> Properties { get; set; }
        public string PropertyStatus { get; set; }
        public string Email { get; set; }
        public DateTime DateTime { get; set; }
        public string PaymentStatus { get; set; } 
        public double TotalAmount { get; set; }
    }
}
