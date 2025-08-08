using Microsoft.EntityFrameworkCore;
using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.BLL.Services.DTOs.Notification;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace PetConnect.BLL.Services.Classes
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPetService _petService;
        private readonly IAttachmentService attachmentSetvice;
        private readonly ICustomerAddedPetsService _customerAddedPetsService;
        private readonly INotificationService notificationService;


        public CustomerService(IUnitOfWork unitOfWork, IPetService petService,
            IAttachmentService attachmentSetvice,ICustomerAddedPetsService customerAddedPetsService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _petService = petService;
            this.attachmentSetvice = attachmentSetvice;
            _customerAddedPetsService = customerAddedPetsService;
            this.notificationService = notificationService;
        }



        public async Task RequestAdoption(CusRequestAdoptionDto adoptionDto,string ReqCustomerId)
        {
            var Pet = _unitOfWork.PetRepository.GetByID(adoptionDto.PetId);

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
            await notificationService.CreateAndSendNotification(adoptionDto.RecCustomerId, new NotificationDTO()
            {
                Message = $"You got a new aoption request for {Pet.Name}",
                Type = NotificationType.Approval
            });
        }
        public async Task<int> DeleteRequestAdoption(DelCusRequestAdoptionDto DeladoptionDto, string ReqCustomerId)
        {
            var AdoptionRecord = _unitOfWork.CustomerPetAdpotionsRepository
                .GetCustomerAdoptionRecord(DeladoptionDto.RecCustomerId, ReqCustomerId, DeladoptionDto.PetId,DeladoptionDto.AdoptionDate);
            var Pet = _unitOfWork.PetRepository.GetByID(DeladoptionDto.PetId);

            if (AdoptionRecord is not null)
            {
                _unitOfWork.CustomerPetAdpotionsRepository.Delete(AdoptionRecord);
                await notificationService.CreateAndSendNotification(DeladoptionDto.RecCustomerId, new NotificationDTO()
                {
                    Message = $"The request to adopt {Pet.Name} was cancelled",
                    Type = NotificationType.Rejection
                });
                return _unitOfWork.SaveChanges();
            }
            return 0;
     
        }


        public IEnumerable<DetailsCustomerRequestAdoption> GetCustomerReqAdoptionsPendingData(string userId)
        {
            var CustomerReqAdoptionsData = _unitOfWork.CustomerPetAdpotionsRepository.GetAllQueryable().Where(CPA => CPA.RequesterCustomerId == userId && CPA.Status== AdoptionStatus.Pending ).ToList();

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
        public IEnumerable<DetailsCustomerReceivedAdoption> GetCustomerRecAdoptionsPendingData(string userId)
        {

            var CustomerReqAdoptionsData = _unitOfWork.CustomerPetAdpotionsRepository.GetAllQueryable().Include(CPA=>CPA.ReceiverCustomer)
                .Include(CPA=>CPA.Pet)
                .ThenInclude(P=>P.Breed)
                .ThenInclude(B=>B.Category)
                .Where(CPA => CPA.ReceiverCustomerId == userId && CPA.Status == AdoptionStatus.Pending)
                .Select(CPA=> new DetailsCustomerReceivedAdoption() { 
                AdoptionDate= CPA.AdoptionDate,
                PetId = CPA.PetId,
                PetName = CPA.Pet.Name,
                RequesterFullName = $"{CPA.RequesterCustomer.FName} {CPA.RequesterCustomer.LName}",
                ReqPhoneNumber = CPA.RequesterCustomer.PhoneNumber,
                PetBreadName = CPA.Pet.Breed.Name,
                PetCategoryName = CPA.Pet.Breed.Category.Name,
                ReqCustomerId = CPA.RequesterCustomerId,
                AdoptionStatus = CPA.Status
                }).ToList();

            return CustomerReqAdoptionsData;
        }

        public async Task<string?> ApproveOrCancelCustomerAdoptionRequest(ApproveORCancelReceivedCustomerRequest approveORCancelCustomerRequestDto,string RecuserId)
        {
            string? result = null;
            var pet = _petService.GetPet(approveORCancelCustomerRequestDto.PetId);

            var CustomerAdoptionsRecord = _unitOfWork.CustomerPetAdpotionsRepository
                .GetCustomerAdoptionRecord(RecuserId, approveORCancelCustomerRequestDto.ReqCustomerId,approveORCancelCustomerRequestDto.PetId,approveORCancelCustomerRequestDto.AdoptionDate);

            if (CustomerAdoptionsRecord == null)
                return result;

            if (approveORCancelCustomerRequestDto.AdoptionStatus == AdoptionStatus.Approved)
            {
                CustomerAdoptionsRecord.Status = AdoptionStatus.Approved;
                result = AdoptionStatus.Approved.ToString();
                pet.Status = PetStatus.Owned;
                _petService.UpdatePetStatus(pet.Id, PetStatus.Owned);
                var CAPRecord = _unitOfWork.CustomerAddedPetsRepository.DeleteCustomerAddedPetRecord(approveORCancelCustomerRequestDto.PetId, RecuserId);
                _customerAddedPetsService.RegisterCustomerPetAddition(approveORCancelCustomerRequestDto.ReqCustomerId, approveORCancelCustomerRequestDto.PetId);
                _unitOfWork.CustomerPetAdpotionsRepository.RemoveSingleReq(RecuserId, approveORCancelCustomerRequestDto.ReqCustomerId, approveORCancelCustomerRequestDto.PetId);

                var otherRequesterIds = _unitOfWork.CustomerPetAdpotionsRepository.RemoveOtherRequestsForPet(approveORCancelCustomerRequestDto.PetId, approveORCancelCustomerRequestDto.ReqCustomerId);
                await notificationService.CreateAndSendNotification(approveORCancelCustomerRequestDto.ReqCustomerId, new NotificationDTO()
                {
                    Message = $"Congratulations! Your request to adopt {pet.Name} has been approved.",
                    Type = NotificationType.Approval
                });
                foreach (var requesterId in otherRequesterIds)
                {
                 await notificationService.CreateAndSendNotification(requesterId, new NotificationDTO()
                    {
                        Message = $"Sorry, the pet {pet.Name} has been adopted by someone else.",
                        Type = NotificationType.Rejection
                    });
                }

            }

            else if (approveORCancelCustomerRequestDto.AdoptionStatus == AdoptionStatus.Cancelled)
            {

                //CustomerAdoptionsRecord.Status = AdoptionStatus.Cancelled;
                _unitOfWork.CustomerPetAdpotionsRepository.RemoveSingleReq(RecuserId, approveORCancelCustomerRequestDto.ReqCustomerId, approveORCancelCustomerRequestDto.PetId);
                result = AdoptionStatus.Cancelled.ToString();
               await notificationService.CreateAndSendNotification(approveORCancelCustomerRequestDto.ReqCustomerId, new NotificationDTO()
                {
                    Message = $"Sorry, Your request to adopt {pet.Name} has been declined.",
                    Type = NotificationType.Rejection
                });
            }
            _unitOfWork.SaveChanges();
            return result;

        }
      

        public IEnumerable<CustomerOwnedPetsDto> GetCustomerOwnedPets(string UserId)
        {
            List<CustomerOwnedPetsDto> petDatas = new List<CustomerOwnedPetsDto>();
            IEnumerable<Pet> PetList = _unitOfWork.PetRepository.GetAllQueryable()
                                       .Include(p=>p.CustomerAddedPets).Include(p=>p.Breed).ThenInclude(B=>B.Category)
                                       .Where(p => p.CustomerAddedPets.CustomerId == UserId );

            foreach (var Pet in PetList)
            {
               
                petDatas.Add(new CustomerOwnedPetsDto()
                {
                    Name = Pet.Name,
                    ImgUrl = $"/assets/PetImages/{Pet.ImgUrl}",
                    Status = Pet.Status,
                    Id = Pet.Id,
                    Age = Pet.Age,
                    CategoryName = Pet.Breed.Category.Name,

                    

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
                UserName = customer.UserName!,
                FName = customer.FName,
                LName = customer.LName,
                ImgUrl = customer.ImgUrl!,
                Gender = customer.Gender,
                Street = customer.Address.Street,
                City = customer.Address.City,
                Country = customer.Address.Country,
                Email =customer.Email!,
                IsApproved= customer.IsApproved,
                PhoneNumber = customer.PhoneNumber!
            
                
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
            customer.PhoneNumber = dto.PhoneNumber;
            customer.UserName = dto.UserName;   
            

            customer.Address = new Address()
            {
                City = dto.City,
                Street = dto.Street,
                Country = dto.Country
            };
     
            _unitOfWork.CustomerRepository.Update(customer);
           return _unitOfWork.SaveChanges();
        }

        public CustomerDataDto? GetCustomerById(string id)
        {
            Customer c = _unitOfWork.CustomerRepository.GetAll().FirstOrDefault(e => e.Id == id);
            if (c is not null)
            {
                CustomerDataDto customerData = new CustomerDataDto()
                {
                    CustomerId = c.Id,
                    FName = c.FName,
                    LName = c.LName,
                    ImgUrl = c.ImgUrl,
                    City = c.Address.City
                };
                return customerData;
            }
            else { return null; }

        }

    }
}
