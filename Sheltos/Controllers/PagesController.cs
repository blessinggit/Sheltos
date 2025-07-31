using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;

namespace Sheltos.Controllers
{
    public class PagesController : Controller
    {
        private readonly IPagesRepository _pagesrepos;

        public PagesController(IPagesRepository pages)
        {
            _pagesrepos = pages;
        }

        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Terms()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>ContactUs(ContactUsViewModel contactVM)
        {
            if(ModelState.IsValid)
            {
                var contact = new ContactUs
                {
                    Email = contactVM.Email,
                    PhoneNumber = contactVM.PhoneNumber,
                    Name = contactVM.Name,
                    Subject = contactVM.Subject
                };
                await _pagesrepos.AddMessageAsync(contact);
                TempData["SuccessMessage"] = "Message sent successfully!";
            }

            return RedirectToAction("ContactUs");
           
        }
    }
}
