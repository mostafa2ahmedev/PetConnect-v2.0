using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Customer
{
    public class CustomerOwnedPetsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int? Age { get; set; }
        public PetStatus Status { get; set; }
        public string ImgUrl { get; set; } = null!;
        public string CategoryName { get; set; } = null!;

    }
}
