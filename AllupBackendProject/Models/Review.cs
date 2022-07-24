namespace AllupBackendProject.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SendText { get; set; }

        public int ProductId { get; set; }
        public Product Products { get; set; }
    }
}
