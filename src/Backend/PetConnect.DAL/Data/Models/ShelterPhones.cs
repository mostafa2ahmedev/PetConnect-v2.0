using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class ShelterPhones
    {
        public int ShelterId { get; set; }
        public string Phone { get; set; } = null!;
        public Shelter Shelter { get; set; } = null!;
    }
}
