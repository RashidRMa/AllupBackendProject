using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllupBackendProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PaymantMethod { get; set; }
        public string TrackingId { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<OrderItem> OrderItems { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Approved,
        Shipped,
        Finished,
        Canceled

    }
}
