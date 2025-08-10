using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class BlogCategory
    {

        public Guid BlogId { get; set; }
        public int PetCategoryId { get; set; }
        public PetCategory PetCategory { get; set; } = null!;
        public Blog Blog { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
