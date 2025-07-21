using PetConnect.BLL.Common.AttachmentServices;
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



        public void RequestAdoption(CusRequestAdoptionDto adoptionDto)
        {
            var CusReqAdoption = new CustomerPetAdoptions()
            {
                RequesterCustomerId = adoptionDto.ReqCustomerId,
                ReceiverCustomerId = adoptionDto.RecCustomerId,
                PetId = adoptionDto.PetId,
                Status = AdoptionStatus.Pending,
                AdoptionDate = DateTime.Now,

            };
            _unitOfWork.CustomerPetAdpotionsRepository.Add(CusReqAdoption);
            _unitOfWork.SaveChanges();
        }
        public List<DetailsCustomerRequestAdoption> GetCustomerReqAdoptionsData(string userId)
        {
            var CustomerReqAdoptionsData = _unitOfWork.CustomerPetAdpotionsRepository.GetAll().Where(CPA => CPA.RequesterCustomerId == userId).ToList();

            List<DetailsCustomerRequestAdoption> detailsCustomerRequestAdoption = new List<DetailsCustomerRequestAdoption>() { };

            foreach (var item in CustomerReqAdoptionsData)
            {
                var Pet = _unitOfWork.PetRepository.GetByID(item.PetId);
                var Bread = _unitOfWork.PetBreedRepository.GetByID(Pet.BreedId);
                var Category = _unitOfWork.PetCategoryRepository.GetByID(Bread.CategoryId);

                detailsCustomerRequestAdoption.Add(new DetailsCustomerRequestAdoption()
                {
                    AdoptionDate = item.AdoptionDate,
                    Status = item.Status,
                    PetName = Pet.Name,
                    PetBreadName = Bread.Name,
                    PetCategoryName = Category.Name
                });
            }

            return detailsCustomerRequestAdoption;
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
