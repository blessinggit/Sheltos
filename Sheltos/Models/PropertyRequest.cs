using System.ComponentModel.DataAnnotations;

namespace Sheltos.Models
{
    public class PropertyRequest
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        public int PropertyId { get; set; } 
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
