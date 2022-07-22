using System;
using System.Collections.Generic;

namespace AllupBackendProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Address { get; set; }
        public string Email { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Shipped,

    }
}
