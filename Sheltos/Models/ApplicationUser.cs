using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Sheltos.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; } 
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool ProfileCompleted { get; set; } = false;
        public bool WantsToBeAgent { get; set; } = false;

        public virtual ICollection<ChatMessage>? SentMessages { get; set; }
        public virtual ICollection<ChatMessage>? ReceivedMessages { get; set; }
    }
}
