using System.ComponentModel.DataAnnotations;

namespace Sheltos.Models
{
    public class Card
    {
        public int Id { get; set; }

        public string? CardName { get; set; }
        [MaxLength(20)]
        public string? CardNumber { get; set; }
        [MaxLength(3)]
        public string? CVV { get; set; }

        public string? CardType { get; set; } 
        [DataType(DataType.Date)]
        public DateTime Expiry { get; set; }

        public bool IsDefault { get; set; }

        public string? UserId { get; set; }
    }
}
