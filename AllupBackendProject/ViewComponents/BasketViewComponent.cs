using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
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

        public BasketViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string basket = Request.Cookies["basket"];

            List<BasketVM> products;

            if (basket != null)
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

                foreach (var item in products)
                {
                    Product product = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == item.Id);
                    item.Price = product.Price;
                    item.Name = product.Name;
                    item.ImgUrl = product.ProductImages.Find(p => p.IsMain == true).ImageUrl;
                }

            }
            else
            {
                products = new List<BasketVM>();
            }

            double total = 0;

            if (products.Count > 0)
            {
                foreach (BasketVM pr in products)
                {
                    total += pr.Price * pr.Count;
                    pr.SubTotal += pr.Price * pr.Count;
                    pr.BasketCount += pr.Count;
                }
            }
            ViewBag.Total = String.Format("{0:0.00}", total); 
            

            return View(await Task.FromResult(products));
        }
    }
}
