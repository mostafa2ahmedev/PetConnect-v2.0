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

        public void RequestAdoption(CusRequestAdoptionDto adoptionDto);

        public List<DetailsCustomerRequestAdoption> GetCustomerReqAdoptionsData(string userId);

        public CustomerProfileDTO GetProfile(string id);
        public IEnumerable<CustomerDetailsDTO> GetAllCustomers();
        public void Delete(string id);
        Task UpdateProfile(CustomerProfileDTO model);
    }
}
