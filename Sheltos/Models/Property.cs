using System.ComponentModel.DataAnnotations.Schema;
using Sheltos.Data.Enum;

namespace Sheltos.Models
{
    public class Property
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Type { get; set; }

        public string? Description { get; set; }

        public int AddressId { get; set; }
       
        public Address? Address { get; set; }

        public double Price { get; set; }

        public int Beds { get; set; }

        public int Bathrooms { get; set; }
        public double PropertySize { get; set; }

        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        public PropertyStatus? PropertyStatus { get; set; } 

        public int AgentId { get; set; }
      
        public Agent? Agent { get; set; }

        public ICollection<PropertyImage>? Gallery { get; set; } 

        public ICollection<PropertyFeature> Features { get; set; }







    }
}
