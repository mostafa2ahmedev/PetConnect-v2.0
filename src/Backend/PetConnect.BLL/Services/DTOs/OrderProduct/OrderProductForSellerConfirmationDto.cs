using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.OrderProduct
{
    public class OrderProductForSellerConfirmationDto
    {
        //orderProduct
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public OrderProductStatus OrderProductStatus { get; set; }
        //order
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        //Product
        public string ProductName { get; set; } = null!;
        public string ProductImgUrl { get; set; } = null!;

 
   


        //Customer
        public string CustomerId { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
      
    }
}
