using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImgUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        
        public int ProductTypeId { get; set; }
        public ProductType Producttype { get; set; } = null!;

        public ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();

        public string? SellerId { get; set; }
        public Seller? Seller { get; set; }


    }
}
