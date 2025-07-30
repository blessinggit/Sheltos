namespace Sheltos.Models
{
    public class PendingAgent
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string? ImageUrl { get; set; }

        public string Address { get; set; }

        public string Qualifications { get; set; }

        public string NinNo { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsApproved { get; set; } = false;
        public string? UserId { get; set; }
       public DateTime? Submitted { get;set; } = DateTime.UtcNow;
       
    }
}
