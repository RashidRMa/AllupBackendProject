using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace AllupBackendProject.Controllers
{
    public class SubscribeController : Controller
    {
        private readonly AppDbContext _context;

        public SubscribeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Subscribe()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Subscribe([FromForm] Subscription subscription)
        {

            Subscription newSubs = new Subscription();

            newSubs.Email = subscription.Email;

            _context.Add(newSubs);
            _context.SaveChanges();

            return Ok();
        }

    }
}
