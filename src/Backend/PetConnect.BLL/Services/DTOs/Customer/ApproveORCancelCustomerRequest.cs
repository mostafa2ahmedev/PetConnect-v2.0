using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Customer
{
    public class ApproveORCancelCustomerRequest
    {
        public AdoptionStatus AdoptionStatus { get; set; }
        public int PetId { get; set; }
        public string RecCustomerId { get; set; } = null!;

    }
}
