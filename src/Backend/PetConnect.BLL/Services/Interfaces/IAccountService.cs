using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTO.Account;
using PetConnect.BLL.Services.DTOs.Account;
using PetConnect.BLL.Services.Models;
using PetConnect.DAL.Data.Identity;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<ApplicationUser?> SignIn(SignInDTO model);
        public Task<List<string>> GetAllRolesAsync();
        public Task<RegistrationResult> DoctorRegister(DoctorRegisterDTO model);
        public Task<RegistrationResult> CustomerRegister([FromForm]CustomerRegisterDTO model);

        public Task<RegistrationResult> SellerRegister([FromForm] SellerRegisterDto model);


    }
}
