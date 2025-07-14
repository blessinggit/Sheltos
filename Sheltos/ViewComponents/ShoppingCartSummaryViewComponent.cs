using Microsoft.AspNetCore.Mvc;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;
namespace Sheltos.ViewComponents
{
    public class ShoppingCartSummaryViewComponent : ViewComponent
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        public ShoppingCartSummaryViewComponent(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }
        public IViewComponentResult Invoke()
        {
            var items = _shoppingCartRepository.GetItems();
            var viewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = items.Select(item => new ShoppingCartItemViewModel
                {
                    PropertyId = item.Property.Id,
                    Title = item.Property.Title,
                    Price = item.Property.Price,
                    ImageUrl = item.Property.Gallery?.FirstOrDefault()?.ImageUrl ?? string.Empty
                }).ToList(),
                TotalPrice = _shoppingCartRepository.GetShoppingItemsTotal()
            };
            return View("_Cartdropdown",viewModel);
        }
    }
}
