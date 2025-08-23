using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.ProductType
{
    public class ProductTypeDataDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? BreedId { get; set; }
        public string? BreedName { get; set; }
    }
}
