using PetConnect.DAL.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class Seller : ApplicationUser
    {

        public ICollection<Product> AddedProducts { get; set; } = new HashSet<Product>();
        public ICollection<OrderProduct> ReviewedOrderProduct { get; set; } = new HashSet<OrderProduct>();


    }
}
