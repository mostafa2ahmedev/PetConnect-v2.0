using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int PetPreedId { get; set; }
        public PetBreed petpreed { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
