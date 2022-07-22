using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupBackendProject.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string Email { get; set; }
        public string Desc { get; set; }
        public string ImageUrl { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
