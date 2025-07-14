namespace Sheltos.ViewModel
{
    public class AgentDetailsViewModel
    {
        public int AgentId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Image { get; set; }
        public string Address { get; set; }
        public int PropertyCount { get; set; }
        public List<PropertyViewModel> Properties { get; set; }
    }
}
