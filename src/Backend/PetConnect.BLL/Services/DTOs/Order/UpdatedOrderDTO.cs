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
        [Required(ErrorMessage ="Id is Required")]
        public int ID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
    }
}
