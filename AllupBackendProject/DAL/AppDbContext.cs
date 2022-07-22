using AllupBackendProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AllupBackendProject.DAL
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Banner> Banners { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Bio> Bios { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagBlog> TagBlogs { get; set; }
        public DbSet<TagProduct> TagProducts { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<WishList> WishLists { get; set; }


    }
}
