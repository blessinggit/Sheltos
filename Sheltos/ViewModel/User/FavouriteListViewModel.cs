namespace Sheltos.ViewModel.User
{
    public class FavouriteListViewModel
    {
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        
        public double Price { get; set; }
        public int Beds { get; set; }
        public int Bathrooms { get; set; }
        public double PropertySize { get; set; }
        public DateTime DateTime { get; set; }
        
        public List<string> GalleryImages { get; set; } = new();
        public string PropertyStatus { get; set; }

        public bool IsFavourite { get; set; }
        
    }
}
