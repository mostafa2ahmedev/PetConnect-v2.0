using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Address
{
    public class AddressDto
    {
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Street { get; set; } = null!;
    }
}
