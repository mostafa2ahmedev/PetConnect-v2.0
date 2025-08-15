using PetConnect.BLL.Services.DTOs.Address;
using PetConnect.BLL.Services.DTOs.OrderProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Order
{
    public class OrderToReturnDto
    {

        public int Id { get; set; }
        public required string BuyerEmail { get; set; }
        public DateTime OrderDate  { get; set; }
        public required string Status { get; set; }
        public required AddressDto ShippingAddress { get; set; }

        public int? DeliveryMethodId { get; set; }

        public string? DeliveryMethod { get; set; }

        public virtual required ICollection<OrderItemDto> OrderItems { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        
    }
}
