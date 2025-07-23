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
        public ICollection<CustomerPetAdoptions> RequestedPetAdoptions { get; set; } = new HashSet<CustomerPetAdoptions>();
        public ICollection<CustomerPetAdoptions> ReceivedAdoptions { get; set; } = new HashSet<CustomerPetAdoptions>();
        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();

    }
}
