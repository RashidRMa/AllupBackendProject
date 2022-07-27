using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AllupBackendProject.Controllers
{
    public class DbBasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;

        public DbBasketController(AppDbContext context, UserManager<AppUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        public IActionResult Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null) return RedirectToAction("login", "account");

            Basket basket = _context.Basket
                .Include(x => x.BasketItems)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .FirstOrDefault(x => x.UserId == currentUserId);

            if (basket == null) return RedirectToAction("index", "shop");

            return View(basket);
        }

        public IActionResult AddItem(int? id, string ReturnUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return RedirectToAction("login", "account");

            if (id == null) return NoContent();

            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            Basket basket = _context.Basket.FirstOrDefault(b => b.UserId == userId);

            List<BasketItem> basketItems = _context.BasketItems.Where(b => b.BasketId == basket.Id).ToList();

            if (product.StockCount == 0) return NotFound();

            BasketItem isexsist = basketItems.Find(p => p.ProductId == id);

            if (isexsist == null)
            {
                BasketItem item = new BasketItem();
                item.ProductId = product.Id;
                item.BasketId = basket.Id;
                item.Count = 1;

                _context.Add(item);
            }
            else
            {
                isexsist.Count ++;
            }

            _context.SaveChanges();

           
            if (ReturnUrl != null) return Redirect(ReturnUrl);

            return RedirectToAction("index", "shop");

        }

        public IActionResult Remove(int id, string ReturnUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return RedirectToAction("login", "account");

            Basket basket = _context.Basket.FirstOrDefault(b => b.UserId == userId);

            List<BasketItem> basketItems = _context.BasketItems.Where(b => b.BasketId == basket.Id).ToList();

            BasketItem deleteItem = basketItems.FirstOrDefault(p => p.ProductId == id);

            _context.BasketItems.Remove(deleteItem);

            _context.SaveChanges();

            if (ReturnUrl != null) return Redirect(ReturnUrl);

            return RedirectToAction("index", "shop");
        }

        public IActionResult Plus(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return RedirectToAction("login", "account");

            Basket basket = _context.Basket.FirstOrDefault(b => b.UserId == userId);

            List<BasketItem> basketItems = _context.BasketItems.Where(b => b.BasketId == basket.Id).ToList();

            BasketItem increaseItem = basketItems.FirstOrDefault(p => p.ProductId == id);

            if (increaseItem.Count < _context.Products.FirstOrDefault(p => p.Id == id).StockCount)
            {
                _context.BasketItems.FirstOrDefault(b => b.Id == increaseItem.Id).Count++;
                _context.SaveChanges();
            }
            
            return RedirectToAction("index", "dbbasket");
        }

        public IActionResult Minus(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return RedirectToAction("login", "account");

            Basket basket = _context.Basket.FirstOrDefault(b => b.UserId == userId);

            List<BasketItem> basketItems = _context.BasketItems.Where(b => b.BasketId == basket.Id).ToList();

            BasketItem decreaseItem = basketItems.FirstOrDefault(p => p.ProductId == id);

            if (decreaseItem.Count > 1)
            {
                _context.BasketItems.FirstOrDefault(b => b.Id == decreaseItem.Id).Count--;
                _context.SaveChanges();
            }
            else
            {
                _context.BasketItems.Remove(decreaseItem);

                _context.SaveChanges();
            }

            return RedirectToAction("index", "dbbasket");
        }


    }
}
