using PetConnect.BLL.Services.DTO.PetBreadDto;
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
    public class PetBreadService : IPetBreedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PetBreadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public List<GPetBreedDto> GetAllBreeds()
        {
           var petBreeds =_unitOfWork.PetBreedRepository.GetAll().ToList();
            List<GPetBreedDto> GPetBreadDtos = new List<GPetBreedDto>();

            foreach (var PetBread in petBreeds)
            {
               var Category= _unitOfWork.PetCategoryRepository.GetByID(PetBread.CategoryId);
                GPetBreadDtos.Add(new GPetBreedDto() {Id= PetBread.Id,Name=PetBread.Name,CategoryName= Category.Name});
            }
            return GPetBreadDtos;
        }
        public List<GPetBreedDto>? GetBreedsByCategoryId(int CategoryId)
        {
            var Category = _unitOfWork.PetCategoryRepository.GetByID(CategoryId);
            if (Category == null) {
                return null;
            }
           return _unitOfWork.PetBreedRepository.GetAll()
                .Where(B => B.CategoryId == CategoryId)
                .Select(B =>
                 new GPetBreedDto() { Id=B.Id,Name=B.Name,CategoryName=Category.Name }).ToList();

        }
        public GPetBreedDto? GetBreedById(int Id)
        {
            var PetBread = _unitOfWork.PetBreedRepository.GetByID(Id);
     
            if (PetBread is not null) {
                var Category = _unitOfWork.PetCategoryRepository.GetByID(PetBread.CategoryId);
                var GPetBreadDto = new GPetBreedDto() { Id = PetBread.Id, Name = PetBread.Name, CategoryName = Category!.Name};
                return GPetBreadDto;
            }
            return null;
        }

        public int AddPetBread(AddedPetBreedDto AddedPetBread)
        {
            var PetBread = new PetBreed() {
            Name = AddedPetBread.Name,
            CategoryId = AddedPetBread.CategoryId,
            };
            _unitOfWork.PetBreedRepository.Add(PetBread);

            return _unitOfWork.SaveChanges();
        }




        public int UpdatePetBread(UPetBreedDto UPetBread)
        {
            var PetBread = new PetBreed() {
            Id = UPetBread.Id,
            Name= UPetBread.Name,   
            CategoryId=UPetBread.CategoryId,    
            };
             _unitOfWork.PetBreedRepository.Update(PetBread);
            return _unitOfWork.SaveChanges();
        }
        public int DeletePetBreed(int Id)
        {
            var PetBread = _unitOfWork.PetBreedRepository.GetByID(Id);
            if (PetBread is not null)
            {
                _unitOfWork.PetBreedRepository.Delete(PetBread);
                return _unitOfWork.SaveChanges();
            }
            return 0;
        }


    }
}
