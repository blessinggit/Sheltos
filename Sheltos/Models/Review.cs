using System.ComponentModel.DataAnnotations;

namespace Sheltos.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        public string? Comment { get; set; } 
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
    }
}
