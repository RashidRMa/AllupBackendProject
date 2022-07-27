using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<BasketVM> products = GetCookBasket();

            if (products != null)
            {
                foreach (var item in products)
                {
                    Product dbProduct = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == item.Id);
                    item.Name = dbProduct.Name;
                    item.Price = dbProduct.Price;
                    item.ImgUrl = dbProduct.ProductImages.FirstOrDefault(i => i.IsMain == true).ImageUrl;
                    item.CategoryId = dbProduct.CategoryId;
                }
            }
            else
            {
                products = new List<BasketVM>();
            }
            return View(products);
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


        public IActionResult Plus(int id)
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM increasePro = products.Find(p => p.Id == id);
            if (increasePro.Count < _context.Products.FirstOrDefault(p => p.Id == id).StockCount)
            {
                increasePro.Count++;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products));
            return RedirectToAction("index", "basket");
        }

        public IActionResult Minus(int id)
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM decreasePro = products.Find(p => p.Id == id);
            if (decreasePro.Count > 1)
            {
                decreasePro.Count--;
            }
            else
            {
                products.Remove(decreasePro);
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products));
            return RedirectToAction("index", "basket");
        }














        public List<BasketVM> GetCookBasket()
        {
            string basket = Request.Cookies["basket"];

            List<BasketVM> products = basket != null ? JsonConvert.DeserializeObject<List<BasketVM>>(basket)
                : new List<BasketVM>();

            return products;
        }
    }
}
