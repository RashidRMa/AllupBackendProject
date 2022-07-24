using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AllupBackendProject.Controllers
{
    public class WishListController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public WishListController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult AddTo(int id)
        {
            
            
            if (!User.Identity.IsAuthenticated) return BadRequest();
            
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null) return NotFound();

            Product dbProcduct = _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p => p.TagProducts)
                    .FirstOrDefault(p => p.Id == id);

            if (dbProcduct == null) return NotFound();


            IQueryable<WishList> wishList = _context.WishLists.Where(w => w.UserId == currentUserId && w.ProductId == id);

            if (wishList.Count() == 0)
            {
                WishList newWishList = new WishList();
                newWishList.Product = dbProcduct;
                newWishList.UserId = currentUserId;

                _context.WishLists.Add(newWishList);
                _context.SaveChanges();
            }


            ////Default message to user
            //TempData["message"] = "This game is already on your wishlist!";

            ////Check if game is already in user's wishlist
            //IQueryable<WishList> wishList = db.WishLists.Where(w => w.MemberId == CurrentMember.Id && w.GameId == id);
            //if (wishList.Count() == 0)
            //{
            //    //Add new wishlist item if user does not have one for this game
            //    TempData["message"] = "Game was added to your wishlist!";
            //    WishList newWishList = new WishList();
            //    newWishList.Product = id;
            //    newWishList.MemberId = CurrentMember.Id;

            //    db.WishLists.Add(newWishList);
            //    db.SaveChanges();
            //}



            return RedirectToAction("index", "home" ); //, new { id = id }
        }
    }
}
