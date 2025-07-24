using PetConnect.BLL.Services.DTOs.OrderProduct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Order
{
    public class AddedOrderDTO
    {
        [Required(ErrorMessage ="Order Date is Required")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Customer Id is Required")]
        public string CustomerId { get; set; }
        public List<OrderProductDTO> products { get; set; } = new();
        public decimal TotalPrice => products?.Sum(p => p.Price * p.Quantity) ?? 0;
    }
}
