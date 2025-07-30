using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sheltos.Models;

namespace Sheltos.ViewComponents
{
    public class UserImageViewComponent:ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserImageViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View("_Index",user);
        }
    }
}
