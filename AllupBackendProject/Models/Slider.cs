using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupBackendProject.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImgUrl { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
