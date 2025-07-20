using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using PetConnect.BLL.Common.AttachmentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.BLL.Services.DTO.PetCategoryDto;

namespace PetConnect.BLL.Services.Classes
{
    public class PetService : IPetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public PetService(IUnitOfWork unitOfWork , IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;


        }

        

        public async Task <int> AddPet(AddedPetDto addedPet )
        {
            var Image = await _attachmentService.UploadAsync(addedPet.form, "PetImages");
            var PetData = new Pet() {Name = addedPet.Name , ImgUrl = Image
                , BreedId = addedPet.BreedId , IsApproved= false , Ownership =addedPet.Ownership ,
                Status = addedPet.Status };
   
            _unitOfWork.PetRepository.Add(PetData);
           return _unitOfWork.SaveChanges();
        }



        public List<PetDataDto> GetAllPets()
        {
            List<PetDataDto> petDatas = new List<PetDataDto>();
            IEnumerable<Pet> PetList= _unitOfWork.PetRepository.GetAll();



            foreach (var Pet in PetList)
            {
                petDatas.Add(new PetDataDto()
                {
                    Name = Pet.Name,
                    ImgUrl = $"/assets/PetImages/{Pet.ImgUrl}",
                    Status = Pet.Status,
                    Id = Pet.Id,
                });
        }
            return petDatas;
        }

        public PetDetailsDto? GetPet(int id)
        {
            var pet = _unitOfWork.PetRepository.GetByID(id);
            if (pet == null)
                return null;
            var bread =   _unitOfWork.PetBreedRepository.GetByID(pet.BreedId);
            var Category =   _unitOfWork.PetCategoryRepository.GetByID(bread.CategoryId);
            PetDetailsDto Pet = new PetDetailsDto() {Name = pet.Name , IsApproved = pet.IsApproved ,BreadName =bread.Name  ,
            ImgUrl = $"/assets/PetImages/{pet.ImgUrl}", Ownership = pet.Ownership , Status = pet.Status , CategoryName = Category.Name};
            return Pet;
        }

        public IEnumerable<PetDataDto> GetAllPetsByCountForAdoption(int count)
        {
            List<PetDataDto> petDatas = new List<PetDataDto>();
            IEnumerable<Pet> PetList = _unitOfWork.PetRepository.GetAll().Where(P => P.Status == PetStatus.ForAdoption).Take(count) ;



            foreach (var Pet in PetList)
            {
                petDatas.Add(new PetDataDto()
                {
                    Name = Pet.Name,
                    ImgUrl = Pet.ImgUrl,
                    Status = Pet.Status,
                    Id = Pet.Id,
                });
            }
            return petDatas;
        }

        public async Task<int> UpdatePet(UpdatedPetDto UpdatedPet)
        {
            var Image = await _attachmentService.ReplaceAsync("", UpdatedPet.form, "Images");
            var Pet = new Pet()
            {
              Id= UpdatedPet.Id,
              Name=UpdatedPet.Name,
              ImgUrl= Image,
              Ownership = UpdatedPet.Ownership,
              Status = UpdatedPet.Status,
              BreedId = UpdatedPet.BreedId,
              IsApproved= false
  
         };
            _unitOfWork.PetRepository.Update(Pet);
            return _unitOfWork.SaveChanges();
        }

 
        
        public int DeletePet(int id)
        {
            var Pet = _unitOfWork.PetRepository.GetByID(id);
            if (Pet is not null)
            {
                _unitOfWork.PetRepository.Delete(Pet);
                return _unitOfWork.SaveChanges();
            }
            return 0;
        }

 
    }
}
