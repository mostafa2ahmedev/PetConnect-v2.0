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
using static Azure.Core.HttpHeader;

namespace PetConnect.BLL.Services.Classes
{
    public class PetService : IPetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;
        private readonly ICustomerAddedPetsService _customerAddedPetsService;

        public PetService(IUnitOfWork unitOfWork , IAttachmentService attachmentService,ICustomerAddedPetsService customerAddedPetsService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
            _customerAddedPetsService = customerAddedPetsService;
        }

        

        public async Task <int> AddPet(AddedPetDto addedPet ,string CustomerId)
        {
            var Image = await _attachmentService.UploadAsync(addedPet.ImgURL, "PetImages");
            var PetData = new Pet() {Name = addedPet.Name , ImgUrl = Image
                , BreedId = addedPet.BreedId  , IsApproved= false , Ownership =addedPet.Ownership ,
                Status = addedPet.Status,Age=addedPet.Age,Notes = addedPet.Notes! };
   
            _unitOfWork.PetRepository.Add(PetData);

            var result = _unitOfWork.SaveChanges();

            if (result > 0) {
                _customerAddedPetsService.RegisterCustomerPetAddition(CustomerId, PetData.Id);
                result = _unitOfWork.SaveChanges();

            }
            return result;
        }
   


        public IEnumerable<PetDataDto> GetAllPetsWithBelongsToCustomer()
        {
            List<PetDataDto> petDatas = new List<PetDataDto>();
            IEnumerable<Pet> PetList= _unitOfWork.PetRepository.GetPetBreadCategoryDataWithCustomer();



            foreach (var Pet in PetList)
            {

          
                petDatas.Add(new PetDataDto()
                {
                    Name = Pet.Name,
                    ImgUrl = $"/assets/PetImages/{Pet.ImgUrl}",
                    Status = Pet.Status,
                    Id = Pet.Id,
                    Age = Pet.Age,
                    CategoryName = Pet.Breed.Category.Name,
                    CustomerId = Pet.CustomerAddedPets.CustomerId,
                    CustomerName = Pet.CustomerAddedPets.Customer.FName + " " + Pet.CustomerAddedPets.Customer.LName,
                    CustomerCity = Pet.CustomerAddedPets.Customer.Address.City,
                    CustomerCountry = Pet.CustomerAddedPets.Customer.Address.Country,
                    CustomerStreet = Pet.CustomerAddedPets.Customer.Address.Street,
                    Notes = Pet.Notes




                });
             }
            return petDatas;
        }
        public IEnumerable<PetDataDto> GetAllForAdoptionPetsWithCustomerData()
        {
            var PetList = _unitOfWork.PetRepository.GetPetBreadCategoryDataWithCustomer().Where(P=>P.Status == PetStatus.ForAdoption).Select(P=>
            new PetDataDto() {
                Name = P.Name,
                ImgUrl = $"/assets/PetImages/{P.ImgUrl}",
                Status = P.Status,
                Id = P.Id,
                Age = P.Age,
                CategoryName = P!.Name,
                CustomerId = P.CustomerAddedPets.CustomerId,
                CustomerName = P.CustomerAddedPets.Customer.FName + " " + P.CustomerAddedPets.Customer.LName,
                CustomerCity = P.CustomerAddedPets.Customer.Address.City,
                CustomerCountry = P.CustomerAddedPets.Customer.Address.Country,
                CustomerStreet = P.CustomerAddedPets.Customer.Address.Street,
                Notes = P.Notes

            }
            
            ).ToList();

            return PetList;
        }

        public IEnumerable<PetDataDto> GetAllForRescuePetsWithCustomerData()
        {
            var PetList = _unitOfWork.PetRepository.GetPetBreadCategoryDataWithCustomer().Where(P => P.Status == PetStatus.ForRescue).Select(P =>
            new PetDataDto()
            {
                Name = P.Name,
                ImgUrl = $"/assets/PetImages/{P.ImgUrl}",
                Status = P.Status,
                Id = P.Id,
                Age = P.Age,
                CategoryName = P!.Name,
                CustomerId = P.CustomerAddedPets.CustomerId,
                CustomerName = P.CustomerAddedPets.Customer.FName + " " + P.CustomerAddedPets.Customer.LName,
                CustomerCity = P.CustomerAddedPets.Customer.Address.City,
                CustomerCountry = P.CustomerAddedPets.Customer.Address.Country,
                CustomerStreet = P.CustomerAddedPets.Customer.Address.Street,
                Notes = P.Notes
            }

            ).ToList();

            return PetList;
        }
        public PetDetailsDto? GetPet(int id)
        {
            var pet = _unitOfWork.PetRepository.GetPetDetails(id);
            if (pet == null)
                return null;
            var bread =   _unitOfWork.PetBreedRepository.GetByID(pet.BreedId);
            var Category =   _unitOfWork.PetCategoryRepository.GetByID(bread.CategoryId);


            PetDetailsDto Pet = new PetDetailsDto() {Id = pet.Id, Name = pet.Name , IsApproved = pet.IsApproved ,BreadName =bread.Name  ,
            ImgUrl = $"/assets/PetImages/{pet.ImgUrl}", Ownership = pet.Ownership , Status = pet.Status , CategoryName = Category.Name,Age = pet.Age ,
                CustomerId = pet.CustomerAddedPets.CustomerId,
                CustomerName  = pet.CustomerAddedPets.Customer.FName+" "+pet.CustomerAddedPets.Customer.LName,
                CustomerCity = pet.CustomerAddedPets.Customer.Address.City,
                CustomerCountry = pet.CustomerAddedPets.Customer.Address.Country,
                CustomerStreet = pet.CustomerAddedPets.Customer.Address.Street,
                Notes = pet.Notes

            };
            return Pet;
        }

        public IEnumerable<PetDataDto> GetAllPetsByCountForAdoption(int count)
        {
            List<PetDataDto> petDatas = new List<PetDataDto>();
            IEnumerable<Pet> PetList = _unitOfWork.PetRepository.GetPetBreadCategoryDataWithCustomer().Where(P => P.Status == PetStatus.ForAdoption).Take(count);



            foreach (var Pet in PetList)
            {
                var petBread = _unitOfWork.PetBreedRepository.GetByID(Pet.BreedId);
                var petCategory = _unitOfWork.PetCategoryRepository.GetByID(petBread!.CategoryId);
               
                petDatas.Add(new PetDataDto()
                {
                    Name = Pet.Name,
                    ImgUrl = Pet.ImgUrl,
                    Status = Pet.Status,
                    Id = Pet.Id,
                    Age = Pet.Age,
                    CategoryName = petCategory!.Name,
                    CustomerId = Pet.CustomerAddedPets.CustomerId,
                    CustomerName = Pet.CustomerAddedPets.Customer.FName + " " + Pet.CustomerAddedPets.Customer.LName,
                    CustomerCity = Pet.CustomerAddedPets.Customer.Address.City,
                    CustomerCountry = Pet.CustomerAddedPets.Customer.Address.Country,
                    CustomerStreet = Pet.CustomerAddedPets.Customer.Address.Street,
                    Notes = Pet.Notes,
                });
            }
            return petDatas;
        }
        public async Task<int> UpdatePet(UpdatedPetDto updatedPet)
        {
        
            var pet = _unitOfWork.PetRepository.GetByID(updatedPet.Id);
            if (pet == null)
            {
                return 0;
            }

           
            if (updatedPet.ImgURL != null )
            {
                var fileName = await _attachmentService.UploadAsync(updatedPet.ImgURL, "PetImages");
                if (!string.IsNullOrEmpty(fileName))
                {
                    pet.ImgUrl = fileName;
                }
            }

            
            pet.Name = updatedPet.Name;
            pet.Ownership = updatedPet.Ownership;
            pet.Status = updatedPet.Status;
            pet.BreedId = updatedPet.BreedId;
            pet.Age = updatedPet.Age;
            pet.IsApproved = false;
            pet.Notes = updatedPet.Notes;

          
            _unitOfWork.PetRepository.Update(pet);
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
       


        public IEnumerable<PetDetailsDto> GetPetsForCustomer(string CustomerId)
        {
            ICollection<PetDetailsDto> customerPets = new List<PetDetailsDto>();
            var pets = _unitOfWork.PetRepository.GetPetDataWithCustomer().Where(e=>e.CustomerAddedPets.CustomerId == CustomerId);
            if (pets == null || pets.Count() == 0)
                return [];
            foreach (var pet in pets)
            {
                var bread = _unitOfWork.PetBreedRepository.GetByID(pet.BreedId);
                var Category = _unitOfWork.PetCategoryRepository.GetByID(bread.CategoryId);


                PetDetailsDto Pet = new PetDetailsDto()
                {
                    Id = pet.Id,
                    Name = pet.Name,
                    IsApproved = pet.IsApproved,
                    BreadName = bread.Name,
                    ImgUrl = $"/assets/PetImages/{pet.ImgUrl}",
                    Ownership = pet.Ownership,
                    Status = pet.Status,
                    CategoryName = Category.Name,
                    Age = pet.Age,
                    CustomerId = pet.CustomerAddedPets.CustomerId,
                    CustomerName = pet.CustomerAddedPets.Customer.FName + " " + pet.CustomerAddedPets.Customer.LName

                };
                customerPets.Add(Pet);
            }
            return customerPets;
        }
    }
}
