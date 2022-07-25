using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupBackendProject.Areas.Admin.Controllers
{
    [Area("admin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;

        public BrandController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search)
        {
            var brands = search == null ? _context.Brands.ToList() : _context.Brands
               .Where(brand => brand.Name.ToLower().Contains(search.ToLower())).ToList();

            return View(brands);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string newBrand)
        {

            if(newBrand == null) return View();

            Brand brand = new Brand();
            brand.Name = newBrand;
            brand.CreatedAt = DateTime.Now;

            _context.Brands.Add(brand);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();

            Brand brand = _context.Brands.Find(id);

            if (brand == null) return NotFound();


            return View(brand);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Brand brand)
        {
            if (!ModelState.IsValid) return View();

            Brand newBrand = _context.Brands.Find(brand.Id);

            if(newBrand == null) return NotFound();

            Brand dbBrandName = _context.Brands.FirstOrDefault(x => x.Name.ToLower() == newBrand.Name.ToLower());

            if (newBrand != null)
            {
                if (newBrand.Name != dbBrandName.Name)
                {
                    ModelState.AddModelError("Name", "Brand name is exist!");
                    return View();
                }
            }

            dbBrandName.Name = brand.Name;
            dbBrandName.UptadetAt = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction("index");
        }


        public  IActionResult Delete(int id)
        {
            var result =  _context.Brands.Find(id);
            _context.Remove(result);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}
