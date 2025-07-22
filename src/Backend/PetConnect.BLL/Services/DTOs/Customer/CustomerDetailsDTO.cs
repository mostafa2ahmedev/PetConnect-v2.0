using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Customer
{
   public  class CustomerDetailsDTO
    {
        public string Id { get; set; } = null!;
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string ImgUrl { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
