using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sheltos.Data;

namespace Sheltos.Models.Repositories
{
    public class FavouriteRepository:IFavouriteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public FavouriteRepository(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<List<int>> GetAllFavouritesById(string userId)
        {
            var favourites = await _context.Favourites
                .Where(f => f.UserId == userId)
                .Select(f => f.PropertyId)
                .ToListAsync();
            return favourites;
        }
        
    }
}
