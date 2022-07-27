using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupBackendProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ProductCode { get; set; }
        public bool IsFeatured { get; set; }
        public bool BestSeller { get; set; }
        public bool NewArrival { get; set; }
        public bool IsDeleted { get; set; }
        public bool InStock { get; set; }
        public double Price { get; set; }
        public Nullable<double> DiscountPrice { get; set; }
        public Nullable<int> PercentageDiscount { get; set; }
        public Nullable<double> TaxTotal { get; set; }
        public int StockCount { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; } = DateTime.Now;
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UptadetAt { get; set; }



        [NotMapped]
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public List<TagProduct> TagProducts { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<WishList> WishLists { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
