using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.OrderProduct
{
    public class UpdatedOrderProductDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
