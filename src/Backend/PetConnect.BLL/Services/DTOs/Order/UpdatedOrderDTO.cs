using PetConnect.BLL.Services.DTOs.OrderProduct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Order
{
    public class UpdatedOrderDTO
    {
        [Required(ErrorMessage = "Id is Required")]
        public int ID { get; set; }

        public DateTime OrderDate { get; set; }

        public string CustomerId { get; set; }

        public List<UpdatedOrderProductDTO> Products { get; set; } = new();

        public decimal TotalPrice => Products?.Sum(p => p.Price * p.Quantity) ?? 0;
    }
}
