using Microsoft.AspNetCore.Mvc;

namespace Sheltos.Controllers
{
    public class PropertyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
