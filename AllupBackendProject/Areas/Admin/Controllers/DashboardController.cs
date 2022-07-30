using AllupBackendProject.DAL;
using AllupBackendProject.Helpers;
using AllupBackendProject.Models;
using AllupBackendProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;

namespace AllupBackendProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public DashboardController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        [Area("admin")]
        public IActionResult Index()
        {

            var orders = _context.Orders
                .ToList();

            return View(orders);
        }


        //public IActionResult Aproove(int id)
        //{

        //    return View();
        //}



        public IActionResult StatusControl(int id, string status = "Pending")
        {
            Order order = _context.Orders.Find(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Find(userId);

            switch (status)
            {
                case "Pending":
                    order.OrderStatus = OrderStatus.Pending;
                    break;
                case "Approved":
                    order.OrderStatus = OrderStatus.Approved;
                    EmailService emailService = new EmailService(_config.GetSection("ConfirmationParams:Email").Value, _config.GetSection("ConfirmationParams:Password").Value);
                    var emailResult = emailService.SendEmail(user.Email, "Your order Approved", $"Dear {user.UserName}, Your order has been confirmed. Have a great day! ", Helper.SendInvoice(order.Id), $"{order.TrackingId}.pdf"); 
                    break;
                case "Shipped":
                    order.OrderStatus = OrderStatus.Shipped;
                    EmailService emailService2 = new EmailService(_config.GetSection("ConfirmationParams:Email").Value, _config.GetSection("ConfirmationParams:Password").Value);
                    var emailResult2 = emailService2.SendEmail(user.Email, "Your order has been shipped", $"Dear {user.UserName}, Your order has been shipped. Your tracking ID: '{order.TrackingId}'. Have a great day! ");
                    break;
                case "Finished":
                    order.OrderStatus = OrderStatus.Finished;
                    break;
                case "Canceled":
                    order.OrderStatus = OrderStatus.Canceled;
                    break;
            }


            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }



}
