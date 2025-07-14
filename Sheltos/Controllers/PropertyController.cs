using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;

namespace Sheltos.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        public PropertyController(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IActionResult> Index(string? type)
        {
            var properties = await _propertyRepository.AllProperties();
            if (!string.IsNullOrEmpty(type))
            {
                properties = properties
                    .Where(p => p.Type.Equals(type));
            }
            var viewModel = properties.Select(p => new PropertyViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Type = p.Type,
                Description = p.Description,
                AgentName = p.Agent.FullName,
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
            return View(viewModel);
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
                AgentName = property.Agent.FullName,
                Email = property.Agent.Email,
                PhoneNumber = property.Agent.PhoneNumber,
                Image = property.Agent.ImageUrl,
                Address = property.Agent.Address,
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
                AgentName = p.Agent.FullName,
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
            var result = new PropertyDetailViewModel
            {
                Property = viewModel,
                RelatedProperties = relatedViewModel
            };
            return View(result);
        }
    }
}
