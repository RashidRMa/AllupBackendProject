using AllupBackendProject.DAL;
using AllupBackendProject.Extentions;
using AllupBackendProject.Interfaces;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AllupBackendProject.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IPhotoService photoService, IWebHostEnvironment env)
        {
            _context = context;
            _photoService = photoService;
            _env = env;
        }



        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Update(int? id)
        {
            ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.ParentId == null).ToList(), "Id", "Name");

            Category category = _context.Categories.Find(id);


            return View(category);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            Category category = _context.Categories.FirstOrDefault();
            return View(category);
        }

        /// <summary>
        /// Local Methods
        /// </summary>
        /// <returns></returns>

        #region Local Methods Region



        //[HttpPost]
        //public async Task<IActionResult> Create(Category category)
        //{
        //    if (category.Image == null)
        //    {
        //        ModelState.AddModelError("Image", "You must upload a photo");
        //        return View();
        //    }

        //    if (!category.Image.IsImage())
        //    {
        //        ModelState.AddModelError("Image", "You must upload only a image type");
        //        return View();
        //    }

        //    if (category.Image.ValidSize(1000))
        //    {
        //        ModelState.AddModelError("Image", "Image size too large");
        //        return View();
        //    }

        //    if (_context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower()))
        //    {
        //        ModelState.AddModelError("Name", "Category name is exist!");
        //        return View();
        //    }

        //    Category newCategory = new Category
        //    {
        //        ImageUrl = category.Image.SaveImage(_env, "assets/images"),
        //        Name = category.Name,
        //        ParentId = category.ParentId,
        //        CreatedAt = DateTime.Now,

        //    };

        //    await _context.AddAsync(newCategory);
        //    await _context.SaveChangesAsync();


        //    return RedirectToAction("Index");
        //}


        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null) return NotFound();
        //    Category dbCategory = await _context.Categories.FindAsync(id);
        //    if (dbCategory == null) return NotFound();

        //    string path = Path.Combine(_env.WebRootPath, "img", dbCategory.ImageUrl);

        //    Helpers.Helper.DeleteImg(path);

        //    _context.Categories.Remove(dbCategory);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Index");

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(Category category)
        //{
        //    Category dbCategory = await _context.Categories.FindAsync(category.Id);

        //    if (ModelState["Image"] != null)
        //    {
        //        if (!category.Image.IsImage())
        //        {
        //            ModelState.AddModelError("Image", "You must upload only a image type");
        //            return View();
        //        }
        //        if (category.Image.ValidSize(1000))
        //        {
        //            ModelState.AddModelError("Image", "Image size too large");
        //            return View();
        //        }

        //        string path = Path.Combine(_env.WebRootPath, "assets/images", dbCategory.ImageUrl);

        //        Helpers.Helper.DeleteImg(path);
        //        dbCategory.ImageUrl = category.Image.SaveImage(_env, "assets/images");
        //    }

        //    dbCategory.Name = category.Name;
        //    dbCategory.ParentId = category.ParentId;
        //    dbCategory.UptadetAt = DateTime.Now;

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}




        #endregion


        /// <summary>
        /// CLoud Methods
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        #region Cloud Methods Region

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View();



            if (_context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Category name is exist!");
                return View();
            }


            var result = await _photoService.AddPhotoAsync(category.Image);

            if (result.Error != null) return BadRequest(result.Error.Message);

            Category newCategory = new Category
            {
                Name = category.Name,
                CreatedAt = DateTime.Now,
                ImageUrl = result.SecureUrl.AbsoluteUri,
                ParentId = category.ParentId,
                PublicId = result.PublicId
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Category dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null) return NotFound();


            var result = await _photoService.DeletePhotoAsync(dbCategory.PublicId);


            _context.Categories.Remove(dbCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            Category dbCategory = await _context.Categories.FindAsync(category.Id);

            if (_context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Category name is exist!");
                return View();
            }


            
            if (dbCategory == null) return NotFound();

            var result = await _photoService.DeletePhotoAsync(dbCategory.PublicId);

            var newresult = await _photoService.AddPhotoAsync(category.Image);

            if (result.Error != null) return BadRequest(result.Error.Message);


            dbCategory.Name = category.Name;
            dbCategory.UptadetAt = DateTime.Now;
            dbCategory.ImageUrl = newresult.SecureUrl.AbsoluteUri;
            dbCategory.ParentId = category.ParentId;
            dbCategory.PublicId = newresult.PublicId;



            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }





        #endregion























    }
}
