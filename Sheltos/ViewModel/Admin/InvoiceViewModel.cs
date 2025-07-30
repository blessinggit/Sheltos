namespace Sheltos.ViewModel
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phonenumber { get; set; }
        public string InvoiceNumber { get; set; }
       
        public List<InvoiceItemViewModel> Properties { get; set; } = new();
        public string PropertyStatus { get; set; }
        public string Email { get; set; }
        public DateTime DateTime { get; set; }
        public string PaymentStatus { get; set; }
        public double TotalAmount { get; set; }
       
    }
}
