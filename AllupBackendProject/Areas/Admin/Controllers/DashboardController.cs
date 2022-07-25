using Microsoft.AspNetCore.Mvc;

namespace AllupBackendProject.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
