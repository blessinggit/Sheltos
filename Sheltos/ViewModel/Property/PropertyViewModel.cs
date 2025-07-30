using Sheltos.Data.Enum;
using Sheltos.Models;

namespace Sheltos.ViewModel
{
    public class PropertyViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string AgentName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public List<IFormFile>? ImageFile { get; set; } = new();
        public double Price { get; set; }
        public int Beds { get; set; }
        public int Bathrooms { get; set; }
        public double PropertySize { get; set; }
        public DateTime DateTime { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public List<string> GalleryImages { get; set; } = new();
        public string PropertyStatus { get; set; }
        public List<string> Features { get; set; } = new();
        public List<string> AvailableFeatures { get; set; } = new();
        public bool DeleteAllImages { get; set; }
        public bool IsFavourite { get; set; }
    }
}
