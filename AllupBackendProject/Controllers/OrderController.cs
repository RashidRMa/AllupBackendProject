using AllupBackendProject.DAL;
using AllupBackendProject.Helpers;
using AllupBackendProject.Models;
using AllupBackendProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SelectPdf;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace AllupBackendProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        public OrderController(AppDbContext context, UserManager<AppUser> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        public IActionResult Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Order order = new Order();

            var basket = _context.Basket.Include(b => b.BasketItems).ThenInclude(b => b.Product).FirstOrDefault(b => b.UserId == currentUserId);

            if (basket != null)
            {
                ViewBag.BasketItems = basket.BasketItems.ToList();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult Sale(Order order, string radio = "Cash")
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Error has been occured");
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _context.Users.Find(currentUserId);

            Order newOrder = new Order();

            var basket = _context.Basket.Include(b => b.BasketItems).ThenInclude(b => b.Product).FirstOrDefault(b => b.UserId == currentUserId);

            var basketItems = basket.BasketItems.ToList();


            newOrder.Address = order.Address;
            newOrder.City = order.City;
            newOrder.Country = order.Country;
            newOrder.CreatedAt = DateTime.Now;
            newOrder.Email = order.Email;
            newOrder.Firstname = order.Firstname;
            newOrder.Lastname = order.Lastname;
            newOrder.Phone = order.Phone;
            newOrder.AppUserId = currentUserId;
            newOrder.OrderStatus = OrderStatus.Pending;
            newOrder.PaymantMethod = radio;
            newOrder.TrackingId = Helper.RandomString(11);
            

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            foreach (var item in basketItems)
            {
                OrderItem newOrderitem = new OrderItem();

                newOrderitem.ProductId = item.ProductId;
                newOrderitem.OrderId = newOrder.Id;
                newOrderitem.Count = item.Count;
                newOrderitem.ProductPrice = item.Product.Price;
                newOrderitem.Total += item.Product.Price * item.Count;
                newOrder.TotalPrice += newOrderitem.Total;

                _context.OrderItems.Add(newOrderitem);
                _context.BasketItems.Remove(item);
            }


            _context.SaveChanges();

            //EmailService emailService = new EmailService(_config.GetSection("ConfirmationParams:Email").Value, _config.GetSection("ConfirmationParams:Password").Value);
            //var emailResult = emailService.SendEmail(user.Email, "Your order Approved", $"Dear {user.UserName}, Your order has been confirmed. Have a great day! ", Helper.SendInvoice(newOrder.Id), $"{newOrder.TrackingId}.pdf");
            return RedirectToAction("index", "home");
        }

        public IActionResult Orders()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = _context.Orders.Where(o => o.AppUserId == currentUserId).OrderByDescending(x=> x.CreatedAt).ToList();

            return View(orders);
        }

        public IActionResult OrderDetail(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .ThenInclude(o => o.ProductImages)
                .FirstOrDefault(o => o.Id == id);

            return View(order);
        }


        public IActionResult Invoice(int id)
        {
            var order = _context.Orders
               .Include(u => u.AppUser)
               .Include(o => o.OrderItems)
               .ThenInclude(o => o.Product)
               .FirstOrDefault(o => o.Id == id);

            //ViewBag.User = _userManager.Users.FirstOrDefault(x=> x.)

            return View(order);
        }

        
    }
}
