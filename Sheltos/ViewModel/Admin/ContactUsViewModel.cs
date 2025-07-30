namespace Sheltos.ViewModel
{
    public class ContactUsViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsReplied { get; set; } = false;

    }
}
