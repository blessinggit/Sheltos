using Microsoft.Identity.Client;

namespace Sheltos.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public int ZipCode { get; set; }

        public List<Property>? Properties { get; set; }

       
    }
}
