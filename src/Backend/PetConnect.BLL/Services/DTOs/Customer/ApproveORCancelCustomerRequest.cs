using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Customer
{
    public class ApproveORCancelReceivedCustomerRequest
    {      
        public AdoptionStatus AdoptionStatus { get; set; }
        public string AdoptionDate { get; set; } = null!;
        public int PetId { get; set; }
        public string ReqCustomerId { get; set; } = null!;


    }
}
