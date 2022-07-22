using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupBackendProject.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Desc { get; set; }
        public string ImgUrl { get; set; }
        public string WrittenBy { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

        public List<TagBlog> TagBlogs { get; set; }


    }
}
