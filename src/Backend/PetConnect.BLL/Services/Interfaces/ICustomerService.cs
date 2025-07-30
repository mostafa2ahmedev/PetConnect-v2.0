using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface ICustomerService
    {

        public void RequestAdoption(CusRequestAdoptionDto adoptionDto, string ReqCustomerId);
        public string? ApproveOrCancelCustomerAdoptionRequest(ApproveORCancelReceivedCustomerRequest approveORCancelCustomerRequestDto, string userId);
        public int DeleteRequestAdoption(DelCusRequestAdoptionDto DeladoptionDto, string ReqCustomerId);
        public IEnumerable<CustomerOwnedPetsDto> GetCustomerOwnedPets(string UserId);
        public CustomerDetailsDTO? GetProfile(string id);
        public IEnumerable<CustomerDataDto> GetAllCustomers();
        public CustomerDataDto? GetCustomerById(string id);
        public int Delete(string id);
        public IEnumerable<DetailsCustomerRequestAdoption> GetCustomerReqAdoptionsPendingData(string userId);
        public IEnumerable<DetailsCustomerReceivedAdoption> GetCustomerRecAdoptionsPendingData(string userId);
        Task<int> UpdateProfile(UpdateCustomerProfileDTO model,string CustomerId);


 
    }
}
