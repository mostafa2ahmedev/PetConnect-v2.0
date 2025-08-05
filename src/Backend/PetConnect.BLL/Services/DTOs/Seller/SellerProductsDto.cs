using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Seller
{
   public class SellerProductsDto
    {
        public int Id { get; set; }
        public string SellerId { get; set; }
        public string ProductName { get; set; }
        public PetConnect.DAL.Data.Models.ProductType ProductType { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public string? ProductDescription { get; set; }
    }
}
