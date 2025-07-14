namespace Sheltos.Models.Repositories
{
    public interface IShoppingCartRepository
    {
        List<ShoppingCartItems> ShoppingCartItems { get; set; }

        void AddToCart(Property property);
        void RemoveItem(Property property);
        List<ShoppingCartItems> GetItems();
        double GetShoppingItemsTotal();
    }
}