using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerId { get; set; } = null!;
        public Customer customer { get; set; } = null!;

        public ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();

        public OrderStatus OrderStatus { get; set; }

        public Address? ShippingAddress { get; set; }
        public int? DeliveryMethodId { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }

        public string PaymentIntentId { get; set; } = "";
    }
}
