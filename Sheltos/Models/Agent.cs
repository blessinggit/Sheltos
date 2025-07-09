using System.ComponentModel.DataAnnotations.Schema;

namespace Sheltos.Models
{
    public class Agent
    {
        public int AgentId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string ImageUrl { get; set; }

        public int AddressId { get; set; }
       
        public Address? Address { get; set; }

        public ICollection<Property> Properties { get; set; }

    }
}
