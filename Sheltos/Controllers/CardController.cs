using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel.Card;

namespace Sheltos.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private readonly ICardRepository _cardRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CardController> _logger;
        public CardController(ICardRepository cardRepository, UserManager<ApplicationUser> userManager, ILogger<CardController> logger)
        {
            _cardRepository = cardRepository;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var cards = await _cardRepository.GetCardsByUserId(user.Id);
            var viewModel = new CardViewModel();
            viewModel.Cards = cards.Select(c => new CardListViewModel
            {
                Id = c.Id,
                CardName = c.CardName,
                CardNumber = c.CardNumber,
                CardType = c.CardType,
                CVV = c.CVV,
                Expiry = c.Expiry,
                IsDefault = c.IsDefault
            }).ToList();
            viewModel.NewCard = new CardListViewModel();
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        { return View(); }
        [HttpPost]
        public async Task<IActionResult> Add(CardViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                Card card = new Card()
                {
                    CardName = model.NewCard.CardName,
                    CardNumber = model.NewCard.CardNumber,
                    CVV = model.NewCard.CVV,
                    CardType = model.NewCard.CardType,
                    Expiry = model.NewCard.Expiry,
                    IsDefault = model.NewCard.IsDefault,
                    UserId = user.Id
                };
                await _cardRepository.AddCardAsync(card);

            }
            _logger.LogWarning("ModelState is invalid while adding card.");

            foreach (var entry in ModelState)
            {
                var key = entry.Key;
                foreach (var error in entry.Value.Errors)
                {
                    _logger.LogWarning("Validation error on '{Field}': {Error}", key, error.ErrorMessage);
                }

            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(CardViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
           
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var card = await _cardRepository.GetCardByIdAsync(model.NewCard.Id);
                if (card == null || card.UserId != user.Id)
                {
                    ModelState.AddModelError("", "No Card Found");
                    _logger.LogWarning("ModelState is invalid while adding card.");

                    foreach (var entry in ModelState)
                    {
                        var key = entry.Key;
                        foreach (var error in entry.Value.Errors)
                        {
                            _logger.LogWarning("Validation error on '{Field}': {Error}", key, error.ErrorMessage);
                        }

                    }
                    return RedirectToAction("Index");
                }
                card.CardName = model.NewCard.CardName;
                card.CardNumber = model.NewCard.CardNumber;
                card.CVV = model.NewCard.CVV;
                card.CardType = model.NewCard.CardType;
                card.Expiry = model.NewCard.Expiry;
                card.IsDefault = model.NewCard.IsDefault;
                await _cardRepository.UpdateCardAsync(card);
               
            }
            _logger.LogWarning("ModelState is invalid while adding card.");

            foreach (var entry in ModelState)
            {
                var key = entry.Key;
                foreach (var error in entry.Value.Errors)
                {
                    _logger.LogWarning("Validation error on '{Field}': {Error}", key, error.ErrorMessage);
                }

            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var card = await _cardRepository.GetCardByIdAsync(id);
            if (card == null || card.UserId != user.Id)
            {
                ModelState.AddModelError("", "No Card Found");
                return RedirectToAction("Index");
            }
            await _cardRepository.DeleteCardAsync(card);
            return RedirectToAction("Index");
        }


    }
}
