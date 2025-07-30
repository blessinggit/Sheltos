using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sheltos.Data;
using Sheltos.Models;
using Sheltos.ViewModel.User;

namespace Sheltos.Controllers
{
    [Authorize]
    public class FavouriteController : Controller
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly ApplicationDbContext _context;
        public FavouriteController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Add(int Propertyid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var favorited = await _context.Favourites.FirstOrDefaultAsync(f => f.UserId == user.Id && f.PropertyId == Propertyid);
            if (favorited == null)
            {
                // If not favorited, add it
                var favourite = new Favourite
                {
                    UserId = user.Id,
                    PropertyId = Propertyid
                };
                _context.Favourites.Add(favourite);
                await _context.SaveChangesAsync();
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Remove(int propertyid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var favourited = await _context.Favourites.FirstOrDefaultAsync(f => f.UserId == user.Id && f.PropertyId == propertyid);
            if (favourited != null)
            {
                // If favorited, remove it
                _context.Favourites.Remove(favourited);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MyFavourites");
        }
        public async Task<IActionResult> MyFavourites()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var favourite = await _context.Favourites
                .Where(f => f.UserId == user.Id)
                .Include(f => f.Property)
                .ThenInclude(p => p.Address)
                .Include(f => f.Property.Gallery)
                .ToListAsync();
            var viewModel = favourite.Select(f => new FavouriteListViewModel
            {
                PropertyId = f.Property.Id,
                Title = f.Property.Title,
                Address = $"{f.Property.Address.City}, {f.Property.Address.State}, {f.Property.Address.Country}",
                Price = f.Property.Price,
                Beds = f.Property.Beds,
                Bathrooms = f.Property.Bathrooms,
                PropertySize = f.Property.PropertySize,
                DateTime = f.Property.DateTime,
                GalleryImages = f.Property.Gallery?.Select(g => g.ImageUrl).ToList() ?? new List<string>(),
                PropertyStatus = f.Property.PropertyStatus.ToString(),
                IsFavourite = true
            }).ToList();
            return View(viewModel);


        }
    }
}
