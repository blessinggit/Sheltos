namespace Sheltos.ViewModel.Property
{
    public class PropertyindexViewModel
    {
        public string PropertyStatus { get; set; } = "All";

        public string State { get; set; } = "All";
        public List<PropertyViewModel> PropertyViewModels { get; set; } = new();
       
    }
}
