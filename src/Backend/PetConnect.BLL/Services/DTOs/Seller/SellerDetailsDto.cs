using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Seller
{
    public class SellerDetailsDto
    {
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string ImgUrl { get; set; } = null!;
        public Gender Gender { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
