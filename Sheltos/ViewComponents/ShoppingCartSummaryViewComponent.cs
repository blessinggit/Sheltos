using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;
namespace Sheltos.ViewComponents
{
    
    public class ShoppingCartSummaryViewComponent : ViewComponent
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartSummaryViewComponent(IShoppingCartRepository shoppingCartRepository,UserManager<ApplicationUser>userManager)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userManager = userManager;
        }
        [Authorize]
        public IViewComponentResult Invoke()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            
            var items = _shoppingCartRepository.GetItems(userId);
            var viewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = items.Select(item => new ShoppingCartItemViewModel
                {
                    PropertyId = item.Property.Id,
                    Title = item.Property.Title,
                    Price = item.Property.Price,
                    ImageUrl = item.Property.Gallery?.FirstOrDefault()?.ImageUrl ?? string.Empty
                }).ToList(),
                TotalPrice = _shoppingCartRepository.GetShoppingItemsTotal(userId)
            };
            return View("_Cartdropdown",viewModel);
        }
    }
}
