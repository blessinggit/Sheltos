using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sheltos.Models;
using Sheltos.ViewModel.User;

namespace Sheltos.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        public UserController(
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
        }
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editVM)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.FullName = editVM.FullName;
            user.PhoneNumber = editVM.PhoneNumber;
            user.Email = editVM.Email;
            user.Address = editVM.Address;
            user.DateOfBirth = editVM.DateOfBirth;
            user.Gender = editVM.Gender;

            await _userManager.UpdateAsync(user);
            return RedirectToAction("Dashboard");

        }

        [HttpPost]
        public async Task<IActionResult> ChangeImage(IFormFile imageFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string oldFileName = user.ProfileImageUrl;
            if (imageFile != null && imageFile.Length > 0)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(imageFile.FileName);
                string imageFullPath = Path.Combine(_environment.WebRootPath, "users", newFileName);
                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    string oldPath = Path.Combine(_environment.WebRootPath, user.ProfileImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                user.ProfileImageUrl = "/users/" + newFileName;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
