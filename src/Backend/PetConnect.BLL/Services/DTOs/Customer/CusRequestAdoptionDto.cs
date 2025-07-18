using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Customer
{
    public class CusRequestAdoptionDto
    {
        public string CustomerId { get; set; } = null!;
        public int PetId { get; set; }
    }
}
