namespace Sheltos.Models.Repositories
{
    public interface IShoppingCartRepository
    {
        List<ShoppingCartItems> ShoppingCartItems { get; set; }

        void AddToCart(Property property,string userId);
        void RemoveItem(Property property, string userId);
        List<ShoppingCartItems> GetItems(string userId);
        Task ClearCartAsync(string userId);
        Task AddCheckoutAsync(Checkout checkout);
        Task<List<Checkout>> GetCheckoutsAsync();
        Task<Checkout> GetCheckOutByIdAsync(int id);
        Task UpdateCheckoutAsync(Checkout checkout);
        Task DeleteCheckoutAsync(Checkout checkout);

        double GetShoppingItemsTotal(string userId);
    }
}