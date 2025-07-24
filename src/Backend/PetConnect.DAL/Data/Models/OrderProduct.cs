using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class OrderProduct
    {
        public int ProductId { get; set; }
        public Product product { get; set; } = null!;

        public int OrderId { get; set; }
        public Order order { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
