using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AllupBackendProject.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddItem(int? id, string ReturnUrl)
        {
            if (id == null) return NoContent();

            string basket = Request.Cookies["basket"];

            Product product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return NoContent();



            List<BasketVM> products;

            if (basket == null)
            {
                products = new List<BasketVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }

            BasketVM isexsist = products.Find(p => p.Id == id);

            if (isexsist == null)
            {
                BasketVM basketVM = new BasketVM
                {
                    Id = product.Id,
                    Count = 1,
                    Price = product.Price
                };
                products.Add(basketVM);
            }
            else
            {
                isexsist.Count++;
            }


            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products));



            if (products.Count > 0)
            {
                foreach (BasketVM pr in products)
                {
                    pr.SubTotal += pr.Price * pr.Count;
                    pr.BasketCount += pr.Count;
                }
            }
            if (ReturnUrl != null) return Redirect(ReturnUrl);

            return RedirectToAction("index", "shop");
        }


        public IActionResult Remove(int? id, string ReturnUrl)
        {
            string basket = Request.Cookies["basket"];

            List<BasketVM> products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            if (products == null) return NotFound();

            List<BasketVM> productsNew = products.FindAll(p => p.Id != id);

            BasketVM product = products.FirstOrDefault(p => p.Id == id);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(productsNew));

            double subtotal = 0;
            int basketCount = 0;

            if (products.Count > 0)
            {
                foreach (BasketVM pr in products)
                {
                    subtotal += pr.Price * product.BasketCount;
                    basketCount += pr.BasketCount;
                }
            }


            if (ReturnUrl != null) return Redirect(ReturnUrl);

            return RedirectToAction("index", "shop");
        }
    }
}
