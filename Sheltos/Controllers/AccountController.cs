using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sheltos.Models;
using Sheltos.Models.Repositories;
using Sheltos.ViewModel;

namespace Sheltos.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IServiceProvider _serviceProvider;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment environment, IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _serviceProvider = serviceProvider;
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel RegisterVM)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = RegisterVM.Email,
                    Email = RegisterVM.Email,
                };
                var result = await _userManager.CreateAsync(user, RegisterVM.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("CompleteProfile", "Account");
                   
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(RegisterVM);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);
                if (user == null)
                {
                   
                    ModelState.AddModelError(string.Empty, $"The email '{loginVM.Email}' is not recognized.");
                    return View(loginVM);
                }
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);
              
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                   

                    if (roles.Contains("Admin"))
                        return RedirectToAction("Dashboard", "Admin");

                    if (roles.Contains("Agent"))
                        return RedirectToAction("Dashboard", "Agent");
                    if (!user.ProfileCompleted)
                    {
                        return RedirectToAction("CompleteProfile", "Account");
                    }
                    return RedirectToAction("Dashboard", "User");


                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginVM);
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult CompleteProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CompleteProfile(CompleteProfileViewModel completeProfileVM)
        {
            if (!ModelState.IsValid)
                return View(completeProfileVM);

            var user = await _userManager.GetUserAsync(User);

            user.FullName = completeProfileVM.FullName;
            user.Address = completeProfileVM.Address;
            user.Gender = completeProfileVM.Gender;
            user.DateOfBirth = completeProfileVM.DateOfBirth;
            user.PhoneNumber = completeProfileVM.PhoneNumber;
            if (completeProfileVM.ImageFile != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(completeProfileVM.ImageFile.FileName);
                string imageFullPath = _environment.WebRootPath + "/users/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    await completeProfileVM.ImageFile.CopyToAsync(stream);
                }
                user.ProfileImageUrl = "/users/" + newFileName;
            }
            user.ProfileCompleted = true;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Dashboard", "User");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(completeProfileVM);
        }
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            var delete = await _userManager.DeleteAsync(user);
            if (delete.Succeeded)
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Index", "Home");


        }
        

    }

}
    
