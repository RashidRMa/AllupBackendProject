using System.Collections.Generic;

namespace AllupBackendProject.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<TagProduct> TagProducts { get; set; }
        public List<TagBlog> TagBlogs { get; set; }
    }
}
