using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class PetBreed
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public PetCategory Category { get; set; } = null!; // Set No Action
        public ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();
        public ICollection<ProductType> PetPreedProducts { get; set; } = new HashSet<ProductType>();
    }
}
