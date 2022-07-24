using AllupBackendProject.Models;
using System.Collections.Generic;

namespace AllupBackendProject.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Category> Categories { get; set; }
        public List<Banner> Banners { get; set; }
        public List<Product> Products { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Testimonial> Testimonials { get; set; }

        public List<Product> MobilephoneTabs { get; set; }
        public List<Product> TVAudioVideos { get; set; }
        public List<Product> Computers { get; set; }

        public List<Product> IsFeatured { get; set; }
        public List<Product> BestSeller { get; set; }
        public List<Product> NewArrival { get; set; }

        public List<ProductImage> ProductImages { get; set; }
        public List<Brand> Brands { get; set; }

    }
}
