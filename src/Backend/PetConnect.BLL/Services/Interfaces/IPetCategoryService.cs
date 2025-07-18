using PetConnect.BLL.Services.DTO.PetCategoryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IPetCategoryService
    {
        int AddPetCategory(AddedPetCategoryDTO AddedPetCategoryDTO);
        StatisticsGPetCategoryDto GetAllCategoriesWithStatistics(bool withTracking = false);
        IEnumerable<GPetCategoryDto> GetAllCategories(bool withTracking = false);
        GPetCategoryDto? GetPetCategoryById(int id);
        int UpdatePetCategory(UPetCategoryDto AUDPetCategoryDto);
        int DeletePetCategory(int id);



    }
}
