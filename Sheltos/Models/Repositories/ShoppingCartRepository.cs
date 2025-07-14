using Microsoft.EntityFrameworkCore;
using Sheltos.Data;

namespace Sheltos.Models.Repositories
{
    public class ShoppingCartRepository: IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public string? ShoppingCartId { get; set; }

        public List<ShoppingCartItems> ShoppingCartItems { get; set; }
        public static ShoppingCartRepository GetCart(IServiceProvider service)
        {
            ISession? session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            ApplicationDbContext context = service.GetService<ApplicationDbContext>() ?? throw new Exception("Error initializing");
            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);
            return new ShoppingCartRepository(context) { ShoppingCartId = cartId };
        }
        public void AddToCart(Property property)
        {
             var item = _context.ShoppingCartItems.Include(x => x.Property)
                .ThenInclude(p => p.Address)
                .SingleOrDefault(x => x.Property.Id == property.Id && x.ShoppingCartId == ShoppingCartId);
            if (item == null)
            {
                item = new ShoppingCartItems
                {
                    ShoppingCartId = ShoppingCartId,
                    PropertyId = property.Id,
                    
                    Quantity = 1
                };
                _context.ShoppingCartItems.Add(item);
            }
            _context.SaveChanges();
        }
        public void RemoveItem(Property property)
        {
            var items = _context.ShoppingCartItems.FirstOrDefault(x => x.Property.Id == property.Id && x.ShoppingCartId == ShoppingCartId);
            if (items != null)
            {
                _context.ShoppingCartItems.Remove(items);
            }
            _context.SaveChanges();
        }
        public List<ShoppingCartItems> GetItems()
        {
            if (ShoppingCartItems != null)
                return ShoppingCartItems;
             ShoppingCartItems ??= _context.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId)
                .Include(x => x.Property)
                .ThenInclude(x => x.Address)
                .Include(x => x.Property)
                .ThenInclude(x => x.Gallery)
                .ToList();
            return ShoppingCartItems;

        }
        public double GetShoppingItemsTotal()
        {
            var total = _context.ShoppingCartItems.Where(x => x.ShoppingCartId == ShoppingCartId)
                .Select(i => i.Property.Price).Sum();
            return total;
        }
    }
}
