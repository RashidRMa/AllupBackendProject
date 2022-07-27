using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupBackendProject.ViewComponents
{
    public class BasketViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = _userManager.GetUserId(Request.HttpContext.User);

            double totalPrice = 0;
            int totalCount = 0;

            Basket basket;

            if (currentUserId == null)
            {
                basket = new Basket();
            }
            else
            {
                basket = _context.Basket
                .Include(b => b.BasketItems)
                .ThenInclude(b => b.Product)
                .ThenInclude(b => b.ProductImages)
                .FirstOrDefault(b => b.UserId == currentUserId);

                if (basket == null)
                {
                    basket = new Basket() { UserId = currentUserId };
                    basket.BasketItems = new List<BasketItem>();
                    _context.Add(basket);
                    _context.SaveChanges();
                }

                foreach (var item in basket.BasketItems)
                {
                    totalPrice += item.Count * item.Product.Price;
                    totalCount += item.Count;
                }
            }

            ViewBag.TotalPrice = totalPrice;
            ViewBag.TotalCount = totalCount;

            return View(await Task.FromResult(basket));
        }
    }
}
