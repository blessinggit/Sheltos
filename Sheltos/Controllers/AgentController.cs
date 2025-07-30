using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sheltos.Data.Enum;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;
using Sheltos.ViewModel.Agents;
using Sheltos.ViewModel.User;
using Property = Sheltos.Models.Property;

namespace Sheltos.Controllers
{
    public class AgentController : Controller
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AgentController> _logger;
        private readonly UserManager<ApplicationUser> _usermanager;
        public AgentController(IAgentRepository agentRepository, IPropertyRepository propertyRepository, IWebHostEnvironment environment,UserManager<ApplicationUser> userManager, ILogger<AgentController> logger)
        {
            _agentRepository = agentRepository;
            _propertyRepository = propertyRepository;
            _environment = environment;
            _usermanager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> List()
        {
            var agents = await _agentRepository.GetAllAgentsAsync();

            var viewModel = agents.Select(agent => new AgentListViewModel
            {
                AgentId = agent.AgentId,
                FullName = agent.FullName,
                Email = agent.Email,
                PhoneNumber = agent.PhoneNumber,
                Image = agent.ImageUrl,
                PropertyCount = agent.Properties?.Count() ?? 0
            });
            return View(viewModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            var agent = await _agentRepository.GetAgentByIdAsync(id);
            if (agent == null)
            {
                return NotFound();
            }
            var properties = await _propertyRepository.GetPropertiesByAgentIdAsync(id);
            var viewModel = new AgentDetailsViewModel
            {
                AgentId = agent.AgentId,
                FullName = agent.FullName,
                Email = agent.Email,
                PhoneNumber = agent.PhoneNumber,
                Image = agent.ImageUrl,
                Address = agent.Address,
                PropertyCount = agent.Properties?.Count() ?? 0,
                Properties = properties.Select(p => new PropertyViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Type = p.Type,
                    Description = p.Description,
                    Price = p.Price,
                    Beds = p.Beds,
                    Bathrooms = p.Bathrooms,
                    PropertySize = p.PropertySize,
                    City = p.Address.City,
                    State = p.Address.State,
                    Country = p.Address.Country,
                    PropertyStatus = p.PropertyStatus.ToString(),
                    GalleryImages = p.Gallery.Select(g => g.ImageUrl).ToList()
                }).ToList()
            };
            return View(viewModel);
        }
        public async Task<IActionResult> CreateAgent()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAgent(AgentViewModel agentVM)
        {
            var user = await _usermanager.GetUserAsync(User);
            if (agentVM.ImageFile == null)
            {
                ModelState.AddModelError("", "Image is required");
            }
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogWarning("Validation error on '{Field}': {Error}", key, error.ErrorMessage);
                    }

                }
                return View(agentVM);
            }
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(agentVM.ImageFile!.FileName);

            string imageFullPath = _environment.WebRootPath + "/agents/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                await agentVM.ImageFile.CopyToAsync(stream);
            }
            var pending = new PendingAgent()
            {
                FullName = agentVM.FullName,
                Email = agentVM.Email,
                PhoneNumber = agentVM.PhoneNumber,
                ImageUrl = "/agents/" + newFileName,
                Address = agentVM.Address,
                Qualifications = agentVM.Qualifications,
                NinNo = agentVM.NinNo,
                DateOfBirth = agentVM.DateOfBirth,
                Gender = agentVM.Gender,
               

            };

            await _agentRepository.AddPendingAgent(pending);
            TempData["SuccessMessage"] = "Agent registered successfully!";
            return RedirectToAction("CreateAgent");
        }
        public async Task<IActionResult> DashBoard()
        {
            var agent = await _agentRepository.GetAgentByEmail(User.Identity!.Name!);
            if (agent == null)
            {
                return View("Login","Account");
            }
            var properties = await _propertyRepository.GetPropertiesByAgentIdAsync(agent.AgentId);
            var viewModel = new AgentDashBoardViewModel
            {
                AgentId = agent.AgentId,
                AgentName = agent.FullName,
                AgentEmail = agent.Email,
                DateOfBirth = agent.DateOfBirth,
                AgentPhoneNumber = agent.PhoneNumber,
                AgentImage = agent.ImageUrl,
                AgentAddress = agent.Address,
                PropertyCount = properties.Count(),
               
            };
            return View(viewModel);
        }
        
        public async Task<IActionResult> EditProfile(AgentDashBoardViewModel editVM)
        {
            var agent = await _agentRepository.GetAgentByEmail(User.Identity!.Name!);
            if (agent == null)
            {
                return RedirectToAction("Login", "Account");
            }
           
            agent.FullName = editVM.AgentName;
            agent.PhoneNumber = editVM.AgentPhoneNumber;
            agent.Email = editVM.AgentEmail;
            agent.Address = editVM.AgentAddress;
            agent.DateOfBirth = editVM.DateOfBirth;
            await _agentRepository.UpdateAgentAsync(agent);
            return RedirectToAction("Dashboard");

        }
       
        public async Task<IActionResult> AddProperty()
        {

            var featurenames = await _propertyRepository.GetAllFeaturesAsync();
            var viewModel = new PropertyViewModel
            {
                AvailableFeatures = featurenames
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddProperty(PropertyViewModel propertyVM)
        {
            ModelState.Remove("AgentName");
            ModelState.Remove("Email");
            ModelState.Remove("Phonenumber");
            ModelState.Remove("Image");
            ModelState.Remove("ImageUrl");
            ModelState.Remove("Address");
            var agent = await _agentRepository.GetAgentByEmail(User.Identity!.Name!);

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogWarning("Validation error on '{Field}': {Error}", key, error.ErrorMessage);
                    }

                }
                return View(propertyVM);

            }
            var imagelist = new List<PropertyImage>();
            if (propertyVM.ImageFile is not null && propertyVM.ImageFile.Count > 0)
            {
                foreach (IFormFile imagefile in propertyVM.ImageFile)
                {
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    newFileName += Path.GetExtension(imagefile!.FileName);

                    string imageFullPath = _environment.WebRootPath + "/properties/" + newFileName;
                    using (var stream = System.IO.File.Create(imageFullPath))
                    {
                        await imagefile.CopyToAsync(stream);
                    }
                    imagelist.Add(new PropertyImage
                    {
                        ImageUrl = "/properties/" + newFileName
                    });
                }

            }

            var property = new Property
            {
                AdminId = null,
                AgentId = agent.AgentId,
                Title = propertyVM.Title,
                Type = propertyVM.Type,
                Description = propertyVM.Description,
                Price = propertyVM.Price,
                Beds = propertyVM.Beds,
                Bathrooms = propertyVM.Bathrooms,
                PropertySize = propertyVM.PropertySize,
                DateTime = DateTime.UtcNow,
                Address = new Address()
                {
                    City = propertyVM.City,
                    State = propertyVM.State,
                    Country = propertyVM.Country
                },
                Gallery = imagelist,
                PropertyStatus = Enum.Parse<PropertyStatus>(propertyVM.PropertyStatus),

            };
            if (propertyVM.Features != null && propertyVM.Features.Any())
            {
                var existingFeatures = await _propertyRepository.GetfeaturesByNamesAsync(propertyVM.Features);

                property.Features = existingFeatures.Select(f => new PropertyFeature
                {
                    Feature = f
                }).ToList();
            }
            await _propertyRepository.AddProperty(property);
            _logger.LogInformation("Property added successfully with title: {Title}", property.Title);
            return RedirectToAction("Dashboard", "Agent");
        }
        public async Task<IActionResult> Properties()
        {
            var agent = await _agentRepository.GetAgentByEmail(User.Identity!.Name!);
            var properties = await _propertyRepository.AllPropertiesByAgent(agent.AgentId);
            if (agent == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var viewmodel = properties.Select(p => new PropertyViewModel
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

            }).ToList();

            return View(viewmodel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
            {
                _logger.LogWarning("Property with ID {Id} not found for editing.", id);
                return RedirectToAction("List");
            }
            var viewModel = new PropertyViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Type = property.Type,
                Description = property.Description,
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
                AvailableFeatures = await _propertyRepository.GetAllFeaturesAsync(),
                Features = property.Features.Select(f => f.Feature.Name).ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PropertyViewModel propertyVM)
        {
            var property = await _propertyRepository.GetByIdAsync(propertyVM.Id);
            if (property == null)
            {
                _logger.LogWarning("Property with ID {Id} not found for editing.", propertyVM.Id);
                return RedirectToAction("List");
            }
            if (propertyVM.DeleteAllImages != null && propertyVM.GalleryImages.Any())
            {
                foreach (var imagePath in propertyVM.GalleryImages)
                {
                    var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }

                propertyVM.GalleryImages.Clear();
            }
            var imagelist = new List<PropertyImage>();
            if (propertyVM.ImageFile is not null && propertyVM.ImageFile.Count > 0)
            {
                foreach (IFormFile imagefile in propertyVM.ImageFile)
                {
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    newFileName += Path.GetExtension(imagefile!.FileName);

                    string imageFullPath = _environment.WebRootPath + "/properties/" + newFileName;
                    using (var stream = System.IO.File.Create(imageFullPath))
                    {
                        await imagefile.CopyToAsync(stream);
                    }
                    imagelist.Add(new PropertyImage
                    {
                        ImageUrl = "/properties/" + newFileName
                    });
                }

            }
            property.Title = propertyVM.Title;
            property.Type = propertyVM.Type;
            property.Price = propertyVM.Price;
            property.Bathrooms = propertyVM.Bathrooms;
            property.Beds = propertyVM.Beds;
            property.Description = propertyVM.Description;
            property.PropertySize = propertyVM.PropertySize;
            property.PropertyStatus = Enum.Parse<PropertyStatus>(propertyVM.PropertyStatus);
            property.Address.City = propertyVM.City;
            property.Address.State = propertyVM.State;
            property.Address.Country = propertyVM.Country;
            if (imagelist.Any())
            {
                property.Gallery = imagelist;
            }
            if (propertyVM.Features != null && propertyVM.Features.Any())
            {
                var existingFeatures = await _propertyRepository.GetfeaturesByNamesAsync(propertyVM.Features);

                property.Features = existingFeatures.Select(f => new PropertyFeature
                {
                    Feature = f
                }).ToList();
            }
            await _propertyRepository.UpdateProperty(property);
            return RedirectToAction("List");
        }
        public async Task<IActionResult> DeleteProperty(int Id)
        {
            var property = await _propertyRepository.GetByIdAsync(Id);
            if (property == null)
            {
                _logger.LogWarning("Property with ID {Id} not found for deletion.", Id);
                return NotFound();
            }

            // Delete related image files from wwwroot
            if (property.Gallery != null && property.Gallery.Any())
            {
                foreach (var image in property.Gallery)
                {
                    var fullPath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }

            await _propertyRepository.DeleteProperty(property);
            _logger.LogInformation("Property with ID {Id} deleted successfully.", Id);

            return RedirectToAction("Properties","Agent");
        }
    }
}
