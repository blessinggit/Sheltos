namespace Sheltos.ViewModel
{
    public class HomeViewModel
    {
        public List<PropertyViewModel> LatestForRent { get; set; }
        public List<PropertyViewModel> LatestForSale { get; set; }
        public List<ShoppingCartItemViewModel> ShoppingCartItems { get; set; }
        public double TotalPrice { get; set; }
        
    }
}
