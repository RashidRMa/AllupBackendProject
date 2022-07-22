using System.ComponentModel.DataAnnotations;

namespace AllupBackendProject.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

    }
}
