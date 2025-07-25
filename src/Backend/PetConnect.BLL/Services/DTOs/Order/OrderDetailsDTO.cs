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
        public decimal TotalPrice { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public List<OrderProductDTO> products { get; set; } = new();

        public int ProductQuantity { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

    }
}
