using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Identity;

namespace PetConnect.DAL.Data.Models
{
    public class Customer:ApplicationUser
    {
        public ICollection<CustomerAddedPets> CustomerAddedPets { get; set; } = new HashSet<CustomerAddedPets>();
        public ICollection<CustomerPetAdoptions> CustomerPetAdoptions { get; set; } = new HashSet<CustomerPetAdoptions>();
    }
}
