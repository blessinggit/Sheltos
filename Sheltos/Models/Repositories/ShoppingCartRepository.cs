using Microsoft.EntityFrameworkCore;
using Sheltos.Data;

namespace Sheltos.Models.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ShoppingCartItems> ShoppingCartItems { get; set; }

        public void AddToCart(Property property, string userId)
        {
            var item = _context.ShoppingCartItems.Include(x => x.Property)
               .ThenInclude(p => p.Address)
               .SingleOrDefault(x => x.Property.Id == property.Id && x.UserId == userId);

            if (item == null)
            {
                item = new ShoppingCartItems
                {

                    PropertyId = property.Id,
                    UserId = userId,
                    Quantity = 1
                };

                _context.ShoppingCartItems.Add(item);
            }
            _context.SaveChanges();
        }

        public void RemoveItem(Property property, string userId)
        {
            var items = _context.ShoppingCartItems.FirstOrDefault(x => x.Property.Id == property.Id && x.UserId == userId);
            if (items != null)
            {
                _context.ShoppingCartItems.Remove(items);
            }
            _context.SaveChanges();
        }

        public async Task ClearCartAsync(string userId)
        {
            var items = await _context.ShoppingCartItems
                .Where(x => x.UserId == userId)
                .ToListAsync();

            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public List<ShoppingCartItems> GetItems(string userId)
        {

            if (ShoppingCartItems != null)
                return ShoppingCartItems;

            ShoppingCartItems ??= _context.ShoppingCartItems.Where(s => s.UserId == userId)
               .Include(x => x.Property)
               .ThenInclude(x => x.Address)
               .Include(x => x.Property)
               .ThenInclude(x => x.Gallery)
               .ToList();
            return ShoppingCartItems;
        }

        public double GetShoppingItemsTotal(string userId)
        {
            var total = _context.ShoppingCartItems.Where(x => x.UserId == userId)
                .Select(i => i.Property.Price).Sum();
            return total;
        }

        public async Task AddCheckoutAsync(Checkout checkout)
        {
            await _context.Checkouts.AddAsync(checkout);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Checkout>> GetCheckoutsAsync()
        {
            return await _context.Checkouts
                .Include(c => c.Items)
                .ThenInclude(c => c.Property)
                .ToListAsync();
        }

        public async Task<Checkout> GetCheckOutByIdAsync(int id)
        {
            return await _context.Checkouts
                .Include(c => c.Items)
                .ThenInclude(c => c.Property)
                .ThenInclude(c => c.Agent)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateCheckoutAsync(Checkout checkout)
        {
             _context.Checkouts.Update(checkout);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCheckoutAsync(Checkout checkout)
        {
            _context.Checkouts.Remove(checkout);
            await _context.SaveChangesAsync();
        }
    }
}
