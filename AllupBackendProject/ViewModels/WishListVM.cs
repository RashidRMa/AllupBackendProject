using AllupBackendProject.Models;
using System.Collections.Generic;

namespace AllupBackendProject.ViewModels
{
    public class WishListVM
    {
        public List<Product> Products { get; set; }
        public List<AppUser> AppUsers { get; set; }

        public Product Product { get; set; }
        public AppUser AppUser { get; set; }

    }
}
