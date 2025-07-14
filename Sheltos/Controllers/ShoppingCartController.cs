using Microsoft.AspNetCore.Mvc;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;

namespace Sheltos.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IPropertyRepository _propertyRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,IPropertyRepository propertyRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _propertyRepository = propertyRepository;
        }

        public IActionResult Index()
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
            return View(viewModel);
        }
        public IActionResult AddToShoppingCart(int id)
        {
            var selectedProperty = _propertyRepository.AllPropertys().FirstOrDefault(p => p.Id == id);

            if (selectedProperty != null)
            {
                _shoppingCartRepository.AddToCart(selectedProperty);
            }

            return Redirect(Request.Headers["Referer"].ToString());

        }
        public IActionResult RemoveFromCart(int id)
        {
            var selectedProperty = _propertyRepository.AllPropertys().FirstOrDefault(p => p.Id == id);

            if (selectedProperty != null)
            {
                _shoppingCartRepository.RemoveItem(selectedProperty);
            }

            return Redirect(Request.Headers["Referer"].ToString());

        }

    }
}
