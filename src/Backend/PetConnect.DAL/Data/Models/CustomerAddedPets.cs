using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.DAL.Data.Models
{
    public class CustomerAddedPets
    {
        public string CustomerId { get; set; } = null!;
        public int PetId { get; set; }
        public DateTime AdditionDate { get; set; }
        public AddedStatus Status { get; set; }

        public Pet Pet { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
    }
}
