using Sheltos.Models;
using Sheltos.ViewModel.Property;

namespace Sheltos.ViewModel
{
    public class PropertyDetailViewModel
    {
        public PropertyViewModel Property { get; set; }
        public List<PropertyViewModel> RelatedProperties { get; set; } 
        public PropertyRequest PropertyRequest { get; set; } = new PropertyRequest();
        public List<Review> Review { get; set; } = new ();
        public Reviewinput Reviewinput { get; set; } = new ();
    }
}
