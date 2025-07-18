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
        public string CustomerId { get; set; } = null!;
        public int PetId { get; set; }
        public DateTime AdoptionDate { get; set; }
        public AdoptionStatus Status { get; set; }

        public Pet Pet { get; set; } = null!;
        public Customer Customer{ get; set; } = null!;
    }
}
