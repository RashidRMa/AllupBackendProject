﻿namespace AllupBackendProject.Models
{
    public class TagBlog
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int TagId { get; set; }

        public Blog Blog { get; set; }
        public Tag Tag { get; set; }
    }
}
