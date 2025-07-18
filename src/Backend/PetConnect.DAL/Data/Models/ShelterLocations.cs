using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class ShelterLocations
    {
        public int ShelterId { get; set; }
        public int LocationCode { get; set; }
        public Address Address { get; set; } = null!;

        public Shelter Shelter { get; set; } = null!;

    }
}
