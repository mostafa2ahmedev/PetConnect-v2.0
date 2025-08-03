using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.OrderProduct
{
    public class ShipOrDenyOrderProductDto
    {

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public OrderProductStatus OrderProductStatus { get; set; }
    }
}
