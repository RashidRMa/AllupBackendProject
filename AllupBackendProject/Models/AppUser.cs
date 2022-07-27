using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AllupBackendProject.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Order> Orders { get; set; }
        public Basket Basket { get; set; }
        public List<WishList> WishLists { get; set; }
    }
}
