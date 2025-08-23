using PetConnect.BLL.Services.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Order
{
    public class OrderToCreateDto
    {
        public required string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        //public required AddressDto ShippingAddress { get; set; }
    }
}
