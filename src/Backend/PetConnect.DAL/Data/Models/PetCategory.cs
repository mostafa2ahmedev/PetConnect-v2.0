using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class PetCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<PetBreed> Breeds { get; set; } = new HashSet<PetBreed>();
    }
}
