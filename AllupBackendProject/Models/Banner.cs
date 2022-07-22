using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupBackendProject.Models
{
    public class Banner
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
