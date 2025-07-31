using System.ComponentModel.DataAnnotations.Schema;

namespace Sheltos.Models
{
    public class Agent
    {
        public int AgentId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? ImageUrl { get; set; }

        public string? Address { get; set; }

        public string? Qualifications { get; set; }

        public string? NinNo { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsApproved { get; set; } = false;
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<Property>? Properties { get; set; }

    }
}
