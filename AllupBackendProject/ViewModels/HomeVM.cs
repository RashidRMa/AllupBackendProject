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
        


    }
}
