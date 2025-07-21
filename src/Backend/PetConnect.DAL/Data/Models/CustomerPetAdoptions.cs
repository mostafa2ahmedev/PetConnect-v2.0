using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.DAL.Data.Models
{
    public class CustomerPetAdoptions
    {
        public string RequesterCustomerId { get; set; } = null!;
        public string ReceiverCustomerId { get; set; } = null!;
        public int PetId { get; set; }
        public DateTime AdoptionDate { get; set; }
        public AdoptionStatus Status { get; set; }

        public Pet Pet { get; set; } = null!;
        public Customer RequesterCustomer{ get; set; } = null!;
        public Customer ReceiverCustomer { get; set; } = null!;
    }
}
