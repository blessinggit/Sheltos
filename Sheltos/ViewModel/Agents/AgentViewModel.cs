namespace Sheltos.ViewModel
{
    public class AgentViewModel
    {
        public int AgentId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }

        public string Address { get; set; }

        public string Qualifications { get; set; }

        public string NinNo { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        
    }
}
