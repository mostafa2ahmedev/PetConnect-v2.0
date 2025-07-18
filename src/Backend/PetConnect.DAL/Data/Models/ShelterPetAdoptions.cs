using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.DAL.Data.Models
{
    public class ShelterPetAdoptions
    {
        public int ShelterId { get; set; }
        public int PetId { get; set; }
        public DateTime AddedDate { get; set; }
        public AdoptionStatus Status { get; set; }

        public Pet Pet { get; set; } = null!;
        public Shelter Shelter { get; set; } = null!;
    }
}
