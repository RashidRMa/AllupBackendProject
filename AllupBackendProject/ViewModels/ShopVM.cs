using AllupBackendProject.Models;
using System.Collections.Generic;

namespace AllupBackendProject.ViewModels
{
    public class ShopVM
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public List<Banner> Banners { get; set; }
        public List<Review> Reviews { get; set; }
        public Review Review { get; set; }


    }
}
