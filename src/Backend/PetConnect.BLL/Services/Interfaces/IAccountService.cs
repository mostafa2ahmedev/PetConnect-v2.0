using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.BLL.Services.DTO.Account;
using PetConnect.DAL.Data.Identity;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<ApplicationUser> SignIn(SignInDTO model);
        public Task<List<string>> GetAllRolesAsync();
        public Task<bool> DoctorRegister(DoctorRegisterDTO model);
        public Task<bool> CustomerRegister(CustomerRegisterDTO model);


    }
}
