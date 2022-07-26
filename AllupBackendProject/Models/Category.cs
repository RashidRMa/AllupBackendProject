using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupBackendProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Category Parent { get; set; }
        public List<Category> Children { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string PublicId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; } = DateTime.Now;
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UptadetAt { get; set; }
    }
}
