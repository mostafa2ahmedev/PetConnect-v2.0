using PetConnect.BLL.Services.DTOs.OrderProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Order
{
    public class AddedOrderDTO
    {
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public List<AddedOrderProductDTO> Products { get; set; }
        public decimal TotalPrice => Products?.Sum(p => p.UnitPrice * p.Quantity) ?? 0;
    }
}
