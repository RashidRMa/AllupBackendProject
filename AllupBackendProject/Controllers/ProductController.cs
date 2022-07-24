using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AllupBackendProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int? id, string name)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbProcduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (dbProcduct == null) return NotFound();
            return View(dbProcduct);

        }
    }
}
