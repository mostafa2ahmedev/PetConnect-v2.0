using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Product
{
    public class ProductDetailsDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImgUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = null!;
    }
}
