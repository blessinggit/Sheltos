using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;
using Sheltos.ViewModel.Card;
using Sheltos.ViewModel.ShoppingCart;

namespace Sheltos.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICardRepository _cardrepo;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,IPropertyRepository propertyRepository,
            UserManager<ApplicationUser> userManager,ICardRepository cardrepo)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _propertyRepository = propertyRepository;
            _userManager = userManager;
            _cardrepo = cardrepo;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
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
            return View(viewModel);
        }
        public IActionResult AddToShoppingCart(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _userManager.GetUserId(User);
            var selectedProperty = _propertyRepository.AllPropertys().FirstOrDefault(p => p.Id == id);

            if (selectedProperty != null)
            {
                _shoppingCartRepository.AddToCart(selectedProperty,userId);
            }
            
            return Redirect(Request.Headers["Referer"].ToString());

        }
        public IActionResult RemoveFromCart(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var selectedProperty = _propertyRepository.AllPropertys().FirstOrDefault(p => p.Id == id);

            if (selectedProperty != null)
            {
                _shoppingCartRepository.RemoveItem(selectedProperty,userId);
            }

            return Redirect(Request.Headers["Referer"].ToString());

        }
        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            var userId = _userManager.GetUserId(User);

            var cartItems = _shoppingCartRepository.GetItems(userId);
            var checkoutItems = cartItems.Select(item => new CheckoutItems
            {
                PropertyId = item.Property.Id,
                TotalAmount = item.Property.Price,
            }).ToList();

            
            var viewModel = new CheckOutViewModel
            {
                Items = checkoutItems
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckOutViewModel checkOut)
        {
            var userId = _userManager.GetUserId(User);
            var items = _shoppingCartRepository.GetItems(userId);
            if (items.Count == 0)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
           

            var checkout = new Checkout
            {
                
                FullName = checkOut.FullName,
                Email = checkOut.Email,
                TotalAmount = items.Sum(item => item.Property.Price),
                PhoneNumber = checkOut.PhoneNumber,
                Address = checkOut.Address,
                PaymentStatus = "Pending",
                PaymentMethod = checkOut.PaymentMethod,
                Items = items.Select(item => new CheckoutItems
                {
                    PropertyId = item.Property.Id,
                    TotalAmount = item.Property.Price
                }).ToList(),
               
               
            };

            await _shoppingCartRepository.AddCheckoutAsync(checkout);
            
            await _shoppingCartRepository.ClearCartAsync(userId);
            return RedirectToAction("Index", "Home");
        }

    }
}
