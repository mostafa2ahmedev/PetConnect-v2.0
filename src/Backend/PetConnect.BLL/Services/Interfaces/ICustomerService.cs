using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Customer;
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

        public List<DetailsCustomerRequestAdoption> GetCustomerReqAdoptionsPendingData(string userId);
        public string? ApproveOrCancelCustomerAdoptionRequest(ApproveORCancelCustomerRequest approveORCancelCustomerRequestDto, string userId);
        public IEnumerable<PetDataDto> GetCustomerOwnedPetsForCustomer(string UserId);
        public CustomerProfileDTO GetProfile(string id);
        public IEnumerable<CustomerDetailsDTO> GetAllCustomers();
        public void Delete(string id);
        Task UpdateProfile(CustomerProfileDTO model);

 
    }
}
