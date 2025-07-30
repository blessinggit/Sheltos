using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sheltos.Data;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;

namespace Sheltos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPropertyRepository _propertyrepos;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFavouriteRepository _favouriteRepo;
    
        

        public HomeController(ILogger<HomeController> logger,IPropertyRepository propertyRepository,IShoppingCartRepository shoppingCartRepository,
            UserManager<ApplicationUser> userManager,IFavouriteRepository favouriteRepo)
        {
            _propertyrepos = propertyRepository;
            _logger = logger;
            _shoppingCartRepository = shoppingCartRepository;
            _userManager = userManager;
            _favouriteRepo = favouriteRepo;
           
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var latestRent = await _propertyrepos.GetLatestForRentAsync(3);
            var latestSale = await _propertyrepos.GetLatestForSaleAsync(3);
            var items = _shoppingCartRepository.GetItems(userId);
           
            List<int> favIds = new();

            if (!string.IsNullOrEmpty(userId))
            {
                favIds = await _favouriteRepo.GetAllFavouritesById(userId);
            }
            var itemviewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = items.Select(item => new ShoppingCartItemViewModel
                {
                    PropertyId = item.Property.Id,
                    Title = item.Property.Title,
                    Price = item.Property.Price,
                    ImageUrl = item.Property.Gallery.FirstOrDefault()?.ImageUrl
                }).ToList(),
                TotalPrice = _shoppingCartRepository.GetShoppingItemsTotal(userId)
            };

            var saleViewModel = latestSale.Select(p => new PropertyViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Type = p.Type,
                Description = p.Description,
                AgentName = p.Agent.FullName ?? p.Admin?.FullName,
                Price = p.Price,
                Beds = p.Beds,
                Bathrooms = p.Bathrooms,
                PropertySize = p.PropertySize,
                DateTime = p.DateTime,
                City = p.Address.City,
                State = p.Address.State,
                Country = p.Address.Country,
                PropertyStatus = p.PropertyStatus.ToString(),
                GalleryImages = p.Gallery.Select(g => g.ImageUrl).ToList(),
                IsFavourite = favIds.Contains(p.Id)
            }).ToList();
            var rentViewModel = latestRent.Select(p => new PropertyViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Type = p.Type,
                Description = p.Description,
                AgentName = p.Agent?.FullName ?? p.Admin?.FullName,
                Price = p.Price,
                Beds = p.Beds,
                Bathrooms = p.Bathrooms,
                PropertySize = p.PropertySize,
                DateTime = p.DateTime,
                City = p.Address.City,
                State = p.Address.State,
                Country = p.Address.Country,
                PropertyStatus = p.PropertyStatus.ToString(),
                GalleryImages = p.Gallery.Select(g => g.ImageUrl).ToList(),
                IsFavourite = favIds.Contains(p.Id)

            }).ToList();
            var viewModel = new HomeViewModel
            {
                LatestForSale = saleViewModel,
                LatestForRent = rentViewModel,
                ShoppingCartItems = itemviewModel.ShoppingCartItems,
                TotalPrice = itemviewModel.TotalPrice
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
