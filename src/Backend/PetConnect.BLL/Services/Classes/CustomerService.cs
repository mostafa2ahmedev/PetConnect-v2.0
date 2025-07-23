using Microsoft.EntityFrameworkCore;
using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPetService _petService;
        private readonly IAttachmentService attachmentSetvice;

        public CustomerService(IUnitOfWork unitOfWork, IPetService petService, IAttachmentService attachmentSetvice)
        {
            _unitOfWork = unitOfWork;
            _petService = petService;
            this.attachmentSetvice = attachmentSetvice;
        }



        public void RequestAdoption(CusRequestAdoptionDto adoptionDto,string ReqCustomerId)
        {
            var CusReqAdoption = new CustomerPetAdoptions()
            {
                RequesterCustomerId = ReqCustomerId,
                ReceiverCustomerId = adoptionDto.RecCustomerId,
                PetId = adoptionDto.PetId,
                Status = AdoptionStatus.Pending,
                AdoptionDate = DateTime.Now,

            };
            _unitOfWork.CustomerPetAdpotionsRepository.Add(CusReqAdoption);
            _unitOfWork.SaveChanges();
        }
        public List<DetailsCustomerRequestAdoption> GetCustomerReqAdoptionsPendingData(string userId)
        {
            var CustomerReqAdoptionsData = _unitOfWork.CustomerPetAdpotionsRepository.GetAll().Where(CPA => CPA.RequesterCustomerId == userId && CPA.Status== AdoptionStatus.Pending ).ToList();

            List<DetailsCustomerRequestAdoption> detailsCustomerRequestAdoption = new List<DetailsCustomerRequestAdoption>();

            foreach (var CPA in CustomerReqAdoptionsData)
            {
                var Pet = _unitOfWork.PetRepository.GetByID(CPA.PetId);
                var Bread = _unitOfWork.PetBreedRepository.GetByID(Pet!.BreedId);
                var Category = _unitOfWork.PetCategoryRepository.GetByID(Bread!.CategoryId);

                detailsCustomerRequestAdoption.Add(new DetailsCustomerRequestAdoption()
                {
                    AdoptionDate = CPA.AdoptionDate,
                    AdoptionStatus = CPA.Status.ToString(),
                    PetName = Pet.Name,
                    PetBreadName = Bread.Name,
                    PetCategoryName = Category!.Name,
                    RecCustomerId = CPA.ReceiverCustomerId,
                    PetId = Pet.Id,
                });
            }

            return detailsCustomerRequestAdoption;
        }
        public string? ApproveOrCancelCustomerAdoptionRequest( ApproveORCancelCustomerRequest approveORCancelCustomerRequestDto,string userId)
        {
            string? result = null;
            var CustomerAdoptionsRecord = _unitOfWork.CustomerPetAdpotionsRepository.GetCustomerAdoptionRecord(userId,approveORCancelCustomerRequestDto.RecCustomerId,approveORCancelCustomerRequestDto.PetId);

            if (CustomerAdoptionsRecord == null)
                return result;

            if (approveORCancelCustomerRequestDto.AdoptionStatus == AdoptionStatus.Approved)
            {
                CustomerAdoptionsRecord.Status = AdoptionStatus.Approved;
                result = AdoptionStatus.Approved.ToString();
            }

            else if (approveORCancelCustomerRequestDto.AdoptionStatus == AdoptionStatus.Cancelled) {
                CustomerAdoptionsRecord.Status = AdoptionStatus.Cancelled;
                result = AdoptionStatus.Cancelled.ToString();
            }
            _unitOfWork.SaveChanges();
            return result;


        }

        public IEnumerable<PetDataDto> GetCustomerOwnedPetsForCustomer(string UserId)
        {
            List<PetDataDto> petDatas = new List<PetDataDto>();
            IEnumerable<Pet> PetList = _unitOfWork.PetRepository.GetAllQueryable()
                                       .Include(p=>p.CustomerAddedPets).Include(p=>p.Breed).ThenInclude(B=>B.Category)
                                       .Where(p => p.CustomerAddedPets.CustomerId == UserId);

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
                    CustomerId = Pet.CustomerAddedPets.CustomerId

                });
            }
            return petDatas;
        }









        public CustomerDetailsDTO? GetProfile(string id)
        {
            var customer = _unitOfWork.CustomerRepository.GetByID(id);

            if (customer == null)
                return null; 

            return new CustomerDetailsDTO
            {
                FName = customer.FName,
                LName = customer.LName,
                ImgUrl = customer.ImgUrl,
                Gender = customer.Gender,
                Street = customer.Address.Street,
                City = customer.Address.City,
                Country = customer.Address.Country,
            };
        }



        public IEnumerable<CustomerDataDto> GetAllCustomers()
        {
            return _unitOfWork.CustomerRepository.GetAll()
                .Select(c => new CustomerDataDto
                {
                    CustomerId = c.Id,
                    FName = c.FName,
                    LName = c.LName,
                    ImgUrl = c.ImgUrl,
                    City = c.Address.City
                }).ToList();
        }


        public int Delete(string id)
        {
            var customer = _unitOfWork.CustomerRepository.GetByID(id);
            if (customer is not null)
            {
                _unitOfWork.CustomerRepository.Delete(customer);
            return    _unitOfWork.SaveChanges();
            }
            return 0;
        }



        //update
        public async Task<int> UpdateProfile(UpdateCustomerProfileDTO dto,string CustomerId)
        {
            var customer = _unitOfWork.CustomerRepository.GetByID(CustomerId);
            if (customer == null)
                return 0;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var fileName = await attachmentSetvice.UploadAsync(dto.ImageFile, Path.Combine("img", "person"));
                if (!string.IsNullOrEmpty(fileName))
                {
                    customer.ImgUrl = $"/assets/img/person/{fileName}";
                }
            }

            customer.FName = dto.FName;
            customer.LName = dto.LName;
            customer.Gender = dto.Gender;

            if (customer.Address == null)
                customer.Address = new Address() { 
                City = dto.City,
                Street =  dto.Street

                };


       


            _unitOfWork.CustomerRepository.Update(customer);
           return _unitOfWork.SaveChanges();
        }

     
    }
}
