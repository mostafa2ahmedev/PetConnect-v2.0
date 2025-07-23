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









        public CustomerProfileDTO GetProfile(string id)
        {
            var customer = _unitOfWork.CustomerRepository.GetByID(id);

            if (customer == null)
                return null; // or throw exception

            return new CustomerProfileDTO
            {
                Id = customer.Id,
                FName = customer.FName,
                LName = customer.LName,
                ImgUrl = customer.ImgUrl,
                Gender = customer.Gender.ToString(),
                Street = customer.Address.Street,
                City = customer.Address.City
            };
        }



        public IEnumerable<CustomerDetailsDTO> GetAllCustomers()
        {
            return _unitOfWork.CustomerRepository.GetAll()
                .Select(c => new CustomerDetailsDTO
                {
                    Id = c.Id,
                    FName = c.FName,
                    LName = c.LName,
                    ImgUrl = c.ImgUrl,
                    City = c.Address.City
                }).ToList();
        }


        public void Delete(string id)
        {
            var customer = _unitOfWork.CustomerRepository.GetByID(id);
            if (customer is not null)
            {
                _unitOfWork.CustomerRepository.Delete(customer);
                _unitOfWork.SaveChanges();
            }

        }



        //update
        public async Task UpdateProfile(CustomerProfileDTO dto)
        {
            var customer = _unitOfWork.CustomerRepository.GetByID(dto.Id);
            if (customer == null)
                throw new Exception("Customer not found");

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

            if (customer.Address == null)
                customer.Address = new Address();

            customer.Address.Street = dto.Street;
            customer.Address.City = dto.City;

            if (Enum.TryParse(dto.Gender, out Gender gender))
                customer.Gender = gender;

            _unitOfWork.CustomerRepository.Update(customer);
            _unitOfWork.SaveChanges();
        }

     
    }
}
