using PetConnect.BLL.Services.DTOs.OrderProduct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Order
{
    public class OrderDetailsDTO
    {

        public DateTime OrderDate { get; set; }
        
        public string CustomerName { get; set; } = null!;
        public List<OrderProductDTO> Products { get; set; }
        public decimal TotalPrice => Products?.Sum(p => p.Price * p.Quantity) ?? 0;

    }
}
