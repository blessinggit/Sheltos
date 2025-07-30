using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Sheltos.Data;
using Sheltos.Data.Enum;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;
using Sheltos.ViewModel.Admin;
using Sheltos.ViewModel.Property;
using static System.Net.Mime.MediaTypeNames;
using IEmailSender = Sheltos.Models.Repositories.IEmailSender;

namespace Sheltos.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAgentRepository _agentRepository;
        private readonly IEmailSender _emailSender;
        private readonly IPagesRepository _pagesRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUserRepository _userRepository;
        public AdminController(IPropertyRepository propertyRepository, IWebHostEnvironment environment,
            ILogger<AdminController> logger, UserManager<ApplicationUser> userManager,
            IAgentRepository agentRepository, IEmailSender emailSender, IPagesRepository pagesRepository,
            IShoppingCartRepository shoppingCartRepository, IUserRepository userRepository)
        {
            _propertyRepository = propertyRepository;
            _environment = environment;
            _logger = logger;
            _userManager = userManager;
            _agentRepository = agentRepository;
            _emailSender = emailSender;
            _pagesRepository = pagesRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
        }
        public IActionResult Dashboard()
        {
            var admin = _userRepository.GetAdminAsync();
            var viewModel = new AdminDashBoardViewModel
            {
                
                Email = admin.Result?.Email,
                Role = "Admin",
                TotalProperties = _propertyRepository.AllProperties().Result.Count(),
                TotalUsers = _userManager.Users.Count(),
                TotalPayments = _shoppingCartRepository.GetCheckoutsAsync().Result.Count(),
                PendingPayments = _shoppingCartRepository.GetCheckoutsAsync().Result.Count(p => p.PaymentStatus == "Pending"),
                PaidPayments = _shoppingCartRepository.GetCheckoutsAsync().Result.Count(p => p.PaymentStatus == "Paid"),
                TotalAgents = _agentRepository.GetAllAgentsAsync().Result.Count(),
                LastLogin = DateTime.UtcNow 
            };
            return View(viewModel);

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

            var currentUser = await _userManager.GetUserAsync(User);
            Property property = new Property()
            {
                AdminId = currentUser?.Id,
                AgentId = null,
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
            return RedirectToAction("Dashboard", "Admin");
        }
        public async Task<IActionResult> List(string? searchTerm)
        {
            var properties = await _propertyRepository.AllProperties();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                properties = properties.Where(p =>
                    (!string.IsNullOrEmpty(p.Type) && p.Type.ToLower().Contains(searchTerm))).ToList();
            }
            var userId = _userManager.GetUserId(User);

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
            ViewBag.SearchTerm = searchTerm;    
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

            return RedirectToAction("List");
        }
        public async Task<IActionResult> AddAgent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAgent(AgentViewModel agentVM)
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
            return RedirectToAction("AgentList");
        }
        public async Task<IActionResult> AgentList(string? searchTerm)
        {
            var agents = await _agentRepository.GetAllAgentsAsync();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                agents = agents.Where(p =>
                    (!string.IsNullOrEmpty(p.FullName) && p.FullName.ToLower().Contains(searchTerm))).ToList();
            }
            var viewModel = agents.Select(agent => new AgentListViewModel
            {
                AgentId = agent.AgentId,
                FullName = agent.FullName,
                Email = agent.Email,
                PhoneNumber = agent.PhoneNumber,
                Image = agent.ImageUrl,
                PropertyCount = agent.Properties?.Count() ?? 0
            });
            ViewBag.SearchTerm = searchTerm;
            return View(viewModel);
        }
        public async Task<IActionResult> EditAgent(int id)
        {
            var agent = await _agentRepository.GetAgentByIdAsync(id);
            if (agent == null)
            {
                _logger.LogWarning("Agent with ID {Id} not found for editing.", id);
                return RedirectToAction("AgentList");
            }
            var viewModel = new AgentViewModel
            {
                AgentId = agent.AgentId,
                FullName = agent.FullName,
                Email = agent.Email,
                Gender = agent.Gender,
                PhoneNumber = agent.PhoneNumber,
                ImageUrl = agent.ImageUrl,
                Address = agent.Address,
                Qualifications = agent.Qualifications,
                NinNo = agent.NinNo,
                DateOfBirth = agent.DateOfBirth,
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditAgent(AgentViewModel agentVM)
        {
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
            var agent = await _agentRepository.GetAgentByIdAsync(agentVM.AgentId);
            if (agent == null)
            {
                _logger.LogWarning("Agent with ID {Id} not found for editing.", agentVM.AgentId);
                return RedirectToAction("AgentList");
            }

            if (agentVM.ImageFile != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(agentVM.ImageFile!.FileName);
                string imageFullPath = _environment.WebRootPath + "/agents/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    await agentVM.ImageFile.CopyToAsync(stream);
                }
                if (!string.IsNullOrEmpty(agent.ImageUrl))
                {
                    string oldPath = Path.Combine(_environment.WebRootPath, agent.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                agent.ImageUrl = "/agents/" + newFileName;
            }

            agent.FullName = agentVM.FullName;
            agent.Email = agentVM.Email;
            agent.PhoneNumber = agentVM.PhoneNumber;
            agent.Address = agentVM.Address;
            agent.Qualifications = agentVM.Qualifications;
            agent.NinNo = agentVM.NinNo;
            agent.DateOfBirth = agentVM.DateOfBirth;
            await _agentRepository.UpdateAgentAsync(agent);
            TempData["SuccessMessage"] = "Agent updated successfully!";
            return RedirectToAction("AgentList");
        }
        public async Task<IActionResult> DeleteAgent(int Id)
        {
            var agent = await _agentRepository.GetAgentByIdAsync(Id);
            if (agent == null)
            {
                _logger.LogWarning("Agent with ID {Id} not found for editing.", agent.AgentId);
                return RedirectToAction("AgentList");
            }

            if (agent.ImageUrl != null && agent.ImageUrl.Any())
            {
                var fullPath = Path.Combine(_environment.WebRootPath, agent.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            await _agentRepository.DeleteAgent(agent);
            _logger.LogInformation("Agent with ID {Id} deleted successfully.", Id);

            return RedirectToAction("AgentList");
        }
        public async Task<IActionResult> PendingAgentsList()
        {
            var pendingagents = await _agentRepository.PendingAgentsList();
            return View(pendingagents);
        }
        public async Task<IActionResult> ApproveAgent(int id)
        {
            var pendingAgent = await _agentRepository.GetPendingAgentByIdAsync(id);
            if (pendingAgent == null)
            {
                _logger.LogWarning("Pending agent with ID {Id} not found for approval.", id);
                return RedirectToAction("PendingAgentsList");
            }
            var existingUser = await _userManager.FindByEmailAsync(pendingAgent.Email);
            if (existingUser != null)
            {
                TempData["Error"] = "A user with this email already exists.";
                return RedirectToAction("PendingAgentsList");
            }
            var existingAgent = await _agentRepository.GetAgentByEmailAsync(pendingAgent.Email);
            if (existingAgent != null)
            {
                TempData["Error"] = "An agent with this email already exists.";
                return RedirectToAction("PendingAgentsList");
            }

            var password = Guid.NewGuid().ToString().Substring(0, 8) + "@A1"; // Or a secure method

            var user = new ApplicationUser
            {
                UserName = pendingAgent.Email,
                Email = pendingAgent.Email,
                EmailConfirmed = true,
                FullName = pendingAgent.FullName
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                TempData["Error"] = "User creation failed.";
                return RedirectToAction("PendingAgentsList");
            }
            await _userManager.AddToRoleAsync(user, "Agent");
            Agent agent = new Agent()
            {

                UserId = user.Id,
                FullName = pendingAgent.FullName,
                Email = pendingAgent.Email,
                PhoneNumber = pendingAgent.PhoneNumber,
                ImageUrl = pendingAgent.ImageUrl,
                Address = pendingAgent.Address,
                Gender = pendingAgent.Gender,
                Qualifications = pendingAgent.Qualifications,
                NinNo = pendingAgent.NinNo,
                DateOfBirth = pendingAgent.DateOfBirth,
                IsApproved = true,
            };
            await _agentRepository.AddAgentAsync(agent);
            await _agentRepository.DeletePendingAgent(pendingAgent);


            // Optionally send email here...

            await _emailSender.SendEmailAsync(user.Email, "Agent Approved", $@"
            <p>Hello {user.FullName},</p>
            <p>Your agent registration has been approved.</p>
            <p><b>Login Email:</b> {user.Email}<br><b>Password:</b> {password}</p>
            <p>You can now log in at <a href='https://localhost:7027/Account/Login'>your dashboard</a>.</p>");
            TempData["SuccessMessage"] = "Agent approved successfully!";
            return RedirectToAction("PendingAgentsList");
        }
        public async Task<IActionResult> RejectAgent(int id)
        {
            var pendingAgent = await _agentRepository.GetPendingAgentByIdAsync(id);
            if (pendingAgent == null)
            {
                _logger.LogWarning("Pending agent with ID {Id} not found for rejection.", id);
                return RedirectToAction("PendingAgentsList");
            }
            await _agentRepository.DeletePendingAgent(pendingAgent);
            TempData["SuccessMessage"] = "Pending agent rejected successfully!";
            return RedirectToAction("PendingAgentsList");
        }
        public async Task<IActionResult> MessageList()
        {
            var Messages = await _pagesRepository.GetAllMessagesAsync();
            var message = Messages.Select(msg => new ContactUsViewModel
            {
                Id = msg.Id,
                Name = msg.Name,
                Email = msg.Email,
                Subject = msg.Subject,
                IsReplied = msg.IsReplied
            }).ToList();
            return View(message);

        }
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _pagesRepository.GetMessageByIdAsync(id);
            if (message == null)
            {
                _logger.LogWarning("Message with ID {Id} not found for deletion.", id);
                return RedirectToAction("MessageList");
            }
            await _pagesRepository.DeleteMessageAsync(id);
            TempData["SuccessMessage"] = "Message deleted successfully!";
            return RedirectToAction("MessageList");
        }
        [HttpGet]
        public async Task<IActionResult> PaymentList()
        {
            var payments = await _shoppingCartRepository.GetCheckoutsAsync();
            var payment = payments.Select(p => new PaymentViewModel
            {
                Id = p.Id,
                FullName = p.FullName,
                Email = p.Email,
                Properties = p.Items.Select(i => i.Property.Title).ToList(),
                TotalAmount = p.Items.Sum(i => i.TotalAmount),
                PaymentStatus = p.PaymentStatus.ToString(),
                DateTime = p.DateTime
            }).ToList();
            return View(payment);
        }
        public async Task<IActionResult> TogglePaymentStatus(int id)
        {
            var payment = await _shoppingCartRepository.GetCheckOutByIdAsync(id);
            if (payment == null)
            {
                _logger.LogWarning("Payment with ID {Id} not found for status toggle.", id);
                return NotFound();
            }
            payment.PaymentStatus = payment.PaymentStatus == "Pending" ? "Paid" : "Pending";
            await _shoppingCartRepository.UpdateCheckoutAsync(payment);
            return RedirectToAction("PaymentList");
            
        }
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var payment = await _shoppingCartRepository.GetCheckOutByIdAsync(id);
            if (payment == null)
            {
                _logger.LogWarning("Payment with ID {Id} not found for status toggle.", id);
                return NotFound();
            }
            await _shoppingCartRepository.DeleteCheckoutAsync(payment);
            return RedirectToAction("PaymentList");
        }
        public async Task<IActionResult> GenerateInvoice(int id)
        {
            var payment = await _shoppingCartRepository.GetCheckOutByIdAsync(id);
            if (payment == null)
            {
                _logger.LogWarning("Payment with ID {Id} not found for invoice generation.", id);
                return NotFound();
            }
            var viewModel = new InvoiceViewModel
            {
                Id = payment.Id,
                FullName = payment.FullName,
                Email = payment.Email,
                Address = payment.Address,
                Phonenumber = payment.PhoneNumber,
                InvoiceNumber = $"INV-{payment.Id}-{DateTime.UtcNow:yyyyMMddHH}",
                Properties = payment.Items.Select(i => new InvoiceItemViewModel
                {
                    PropertyTitle = i.Property.Title,
                    Price = i.TotalAmount,
                }).ToList(),
                TotalAmount = payment.Items.Sum(i => i.TotalAmount),
                PaymentStatus = payment.PaymentStatus.ToString(),
                DateTime = DateTime.UtcNow
            };
            if(payment.PaymentStatus == "Paid")
            {
                await SendPaymentConfirmationEmail(viewModel);
            }
            return View(viewModel);
        }
        private async Task SendPaymentConfirmationEmail(InvoiceViewModel payment)
        {
            var invoiceNumber = $"INV-{payment.Id}-{DateTime.UtcNow:yyyyMMddHH}";

            var message = new MailMessage
            {
                From = new MailAddress("nwekeblessing06@gmail.com", "Sheltos Admin"),
                Subject = $"Payment Confirmed – Invoice {invoiceNumber}",
                Body = $@"
                    Dear {payment.FullName},<br/><br/>
                    This is to confirm that we have received your payment for the property you selected.<br/>
                    Your invoice is now ready and available for download or pickup.<br/><br/>
                    <strong>Invoice Number:</strong> {invoiceNumber}<br/>
                    <strong>Total Amount:</strong> ₦{payment.TotalAmount}<br/><br/>
                    Thank you for choosing <strong>Sheltos</strong>.<br/><br/>
                    Warm regards,<br/>
                    Sheltos Team
                ",
                        IsBodyHtml = true
            };

            message.To.Add(payment.Email);

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("nwekeblessing06@gmail.com", "dtqo sccq nqhk emvh"),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }

    
}

