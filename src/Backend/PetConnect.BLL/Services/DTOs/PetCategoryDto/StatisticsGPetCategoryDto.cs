using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO.PetCategoryDto
{
    public class StatisticsGPetCategoryDto
    {

        public int CategoriesCount { get; set; }
        public List<GPetCategoryDto> GPetCategoryDto { get; set; } = new List<GPetCategoryDto>();
    }
}
