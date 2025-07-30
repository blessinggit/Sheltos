using System.Runtime.CompilerServices;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sheltos.Data.Enum;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;
using Sheltos.ViewModel.Property;
using Property = Sheltos.Models.Property;

namespace Sheltos.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFavouriteRepository _favouriteRepo;
        private readonly ILogger<PropertyController> _logger;
        private readonly IWebHostEnvironment _environment;

        public PropertyController(IPropertyRepository propertyRepository, UserManager<ApplicationUser> userManager,
            IFavouriteRepository favouriteRepo, ILogger<PropertyController> logger, IWebHostEnvironment environment)
        {
            _propertyRepository = propertyRepository;
            _userManager = userManager;
            _favouriteRepo = favouriteRepo;
            _logger = logger;
            _environment = environment;
        }

        public async Task<IActionResult> Index(string? type)
        {
            var properties = await _propertyRepository.AllProperties();
            if (!string.IsNullOrEmpty(type))
            {
                properties = properties
                    .Where(p => p.Type.Equals(type));
            }
            var userId = _userManager.GetUserId(User);
            List<int> favIds = new();

            if (!string.IsNullOrEmpty(userId))
            {
                favIds = await _favouriteRepo.GetAllFavouritesById(userId);

            }
            var propertyViewModels = properties.Select(p => new PropertyViewModel
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
            var viewmodel = new PropertyindexViewModel()
            {
                PropertyViewModels = propertyViewModels,
                PropertyStatus = "All",
                State = "All"
            };
            return View(viewmodel);
        }
        public async Task<IActionResult> Details(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            var viewModel = new PropertyViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Type = property.Type,
                Description = property.Description,
                AgentName = property.Agent?.FullName ?? property.Admin?.FullName,
                Email = property.Agent?.Email ?? property.Admin?.Email,
                PhoneNumber = property.Agent?.PhoneNumber ?? property.Admin?.PhoneNumber,
                
                Image = property.Agent?.ImageUrl ?? property.Admin?.ProfileImageUrl,
                Address = property.Agent?.Address ?? property.Admin?.Address,
                Price = property.Price,
                Beds = property.Beds,
                Bathrooms = property.Bathrooms,
                PropertySize = property.PropertySize,
                DateTime = property.DateTime,
                City = property.Address.City,
                State = property.Address.State,
                Country = property.Address.Country,
                PropertyStatus = property.PropertyStatus.ToString(),
                GalleryImages = property.Gallery.Select(g => g.ImageUrl).ToList(),
                Features = property.Features.Select(f => f.Feature.Name).ToList()
            };
            var relatedProperties = await _propertyRepository.GetRelatedProperties(property.Type);
            var relatedViewModel = relatedProperties.Select(p => new PropertyViewModel
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
                GalleryImages = p.Gallery.Select(g => g.ImageUrl).ToList()
            }).ToList();
            var reviews = await _propertyRepository.GetReviews(id);
            var reviewViewModel = reviews.Select(p => new Review
            {
                 Id = p.Id,
                Comment = p.Comment,
                 DateTime = p.DateTime,
                 UserId = p.User.Id,
                 PropertyId =p.PropertyId,
                 User = p.User
            }).ToList();
            var result = new PropertyDetailViewModel
            {
                Review = reviewViewModel,
                Property = viewModel,
                RelatedProperties = relatedViewModel
            };
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> RequestProperty(PropertyDetailViewModel requestVM)
        {
            ModelState.Remove("Property");
            ModelState.Remove("RelatedProperties");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid while adding request.");

                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogWarning("Validation error on '{Field}': {Error}", key, error.ErrorMessage);
                    }

                }
                return View("Details", requestVM);
            }
            var request = new PropertyRequest
            {
                Id = requestVM.PropertyRequest.Id,
                Name = requestVM.PropertyRequest.Name,
                Email = requestVM.PropertyRequest.Email,
                PhoneNumber = requestVM.PropertyRequest.PhoneNumber,
                Subject = requestVM.PropertyRequest.Subject,
                PropertyId = requestVM.PropertyRequest.PropertyId
            };
            await _propertyRepository.AddRequestAsync(request);
            return RedirectToAction("Details", new { id = request.PropertyId });
        }
        [Authorize]
        public async Task<IActionResult> AddReview(PropertyDetailViewModel input)
        {
            ModelState.Remove("Property");
            ModelState.Remove("RelatedProperties");
            ModelState.Remove("PropertyRequest.Name");
            ModelState.Remove("PropertyRequest.Email");
            ModelState.Remove("PropertyRequest.PhoneNumber");
            ModelState.Remove("PropertyRequest.Subject");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid while adding review.");
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogWarning("Validation error on '{Field}': {Error}", key, error.ErrorMessage);
                    }
                }
                return RedirectToAction("Details", new { id = input.Reviewinput.PropertyId});
            }
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                
                return RedirectToAction("Login", "Account");
            }
            var review = new Review
            {
                Comment = input.Reviewinput.Comment,
                UserId = user.Id,
                PropertyId = input.Reviewinput.PropertyId,
                DateTime = DateTime.UtcNow
            };
            _propertyRepository.AddReview(review);
           
            return RedirectToAction("Details", new { id = input.Reviewinput.PropertyId });

        }
        
        public async Task<IActionResult> Delete(int reviewId, int propertyId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {

                return RedirectToAction("Login", "Account");
            }
            var review = await _propertyRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogWarning("Validation error on '{Field}': {Error}", key, error.ErrorMessage);
                    }

                }
                return RedirectToAction("Details", new { id = propertyId });
            }
            if (review.UserId != _userManager.GetUserId(User))
            {
                return RedirectToAction("Login", "Account");
            }
            await _propertyRepository.DeleteReview(review);
            return RedirectToAction("Details", new {id = reviewId});
        }
        public async Task<IActionResult> Search(PropertyindexViewModel filter)
        {
            
            var properties = await _propertyRepository.SearchAsync(filter.State,filter.PropertyStatus);
            if (properties == null || !properties.Any())
            {
                return RedirectToAction("Index");
            }
            var userId = _userManager.GetUserId(User);
            List<int> favIds = new();
            if (!string.IsNullOrEmpty(userId))
            {
                favIds = await _favouriteRepo.GetAllFavouritesById(userId);
            }
            var viewmodel = new PropertyindexViewModel
            {
                PropertyViewModels = properties.Select(p => new PropertyViewModel
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
                }).ToList(),
                PropertyStatus = filter.PropertyStatus,
                State = filter.State
            };
           
            return View("Index", viewmodel);
        }
       
       
    }
}
