using PetConnect.BLL.Services.DTO.PetCategoryDto;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class PetCategoryService : IPetCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PetCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public StatisticsGPetCategoryDto GetAllCategoriesWithStatistics(bool withTracking = false)
        {
            var PetCategoryList = _unitOfWork.PetCategoryRepository.GetAll(withTracking).ToList();

            List<GPetCategoryDto> PetCategoryDtoList = new List<GPetCategoryDto>();

            StatisticsGPetCategoryDto statisticsGPetCategoryDto = new StatisticsGPetCategoryDto();

            foreach (var petCategory in PetCategoryList)
            {
                PetCategoryDtoList.Add(new GPetCategoryDto() { Name = petCategory.Name, Id = petCategory.Id });
            }
            statisticsGPetCategoryDto.CategoriesCount = PetCategoryDtoList.Count;
            statisticsGPetCategoryDto.GPetCategoryDto = PetCategoryDtoList;

            return statisticsGPetCategoryDto;
        }

        public IEnumerable<GPetCategoryDto> GetAllCategories(bool withTracking = false)
        {
            var PetCategoryList = _unitOfWork.PetCategoryRepository.GetAll(withTracking).ToList();

            List<GPetCategoryDto> PetCategoryDtoList = new List<GPetCategoryDto>();
            foreach (var petCategory in PetCategoryList)
            {
                PetCategoryDtoList.Add(new GPetCategoryDto() { Name = petCategory.Name, Id = petCategory.Id });
            }
            return PetCategoryDtoList;
        }
        public int AddPetCategory(AddedPetCategoryDTO AddedPetCategoryDTO)
        {
            var PetCategory = new PetCategory()
            {
                Name = AddedPetCategoryDTO.Name
            };

            _unitOfWork.PetCategoryRepository.Add(PetCategory);
            return _unitOfWork.SaveChanges();

        }



        public GPetCategoryDto? GetPetCategoryById(int id)
        {

            var PetCategory = _unitOfWork.PetCategoryRepository.GetByID(id);
            if (PetCategory is not null)
            {
                return new GPetCategoryDto() { Name = PetCategory.Name, Id = PetCategory.Id };
            }
            return null;

        }

        public int UpdatePetCategory(UPetCategoryDto UPetCategoryDto)
        {
            var PetCategory = new PetCategory()
            {
                Id = UPetCategoryDto.Id,
                Name = UPetCategoryDto.Name
            };

            _unitOfWork.PetCategoryRepository.Update(PetCategory);
            return _unitOfWork.SaveChanges();

        }
        public int DeletePetCategory(int id)
        {
            var PetCategory = _unitOfWork.PetCategoryRepository.GetByID(id);

            if (PetCategory is not null)
            {
                _unitOfWork.PetCategoryRepository.Delete(PetCategory);
                return _unitOfWork.SaveChanges();
            }
            return 0;


        }


    }
}
