using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;

namespace Sheltos.Controllers
{
    public class AgentController : Controller
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IWebHostEnvironment _environment;
        public AgentController(IAgentRepository agentRepository, IPropertyRepository propertyRepository, IWebHostEnvironment environment)
        {
            _agentRepository = agentRepository;
            _propertyRepository = propertyRepository;
            _environment = environment;
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
            if (agentVM.ImageFile == null)
            {
                ModelState.AddModelError("", "Image is required");
            }
            if (!ModelState.IsValid)
            {
                return View(agentVM);
            }
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(agentVM.ImageFile!.FileName);

            string imageFullPath = _environment.WebRootPath + "/agents/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                await agentVM.ImageFile.CopyToAsync(stream);
            }
            Agent agent = new Agent()
            {
                FullName = agentVM.FullName,
                Email = agentVM.Email,
                PhoneNumber = agentVM.PhoneNumber,
                ImageUrl = "/agents/" + newFileName,
                Address = agentVM.Address,
                Qualifications = agentVM.Qualifications,
                NinNo = agentVM.NinNo,
                DateOfBirth = agentVM.DateOfBirth,
                Gender = agentVM.Gender
            };

            await _agentRepository.AddAgentAsync(agent);
            TempData["SuccessMessage"] = "Agent registered successfully!";
            return RedirectToAction("CreateAgent");
        }
    }
}
