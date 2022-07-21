using Microsoft.AspNetCore.Mvc;

namespace AllupBackendProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
