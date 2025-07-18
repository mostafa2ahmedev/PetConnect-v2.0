using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetConnect.BLL.Services.DTO.Account;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Identity;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.BLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly SignInManager<ApplicationUser> signinManager;

        public AccountService(IUnitOfWork _unitOfWork,
            UserManager<ApplicationUser> _userManager,
            RoleManager<ApplicationRole> _roleManager,
            SignInManager<ApplicationUser> _signinManager)
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
            roleManager = _roleManager;
            signinManager = _signinManager;
        }
        public async Task<List<string>> GetAllRolesAsync()
        {
            return await roleManager.Roles.Select(r => r.Name).ToListAsync();
        }

        public async Task<ApplicationUser> SignIn(SignInDTO model)
        {
            ApplicationUser? applicationUser = await userManager.FindByEmailAsync(model.Email);

            if (await userManager.CheckPasswordAsync(applicationUser, model.Password))
            {
                return applicationUser;
            }
            return null;
        }

        public async Task<bool> DoctorRegister(DoctorRegisterDTO model)
        {
            var doctor = new Doctor
            {
                FName = model.FName,
                LName = model.LName,
                Email = model.Email,
                UserName = model.Email,
                ImgUrl = model.ImageUrl,
                Gender = model.Gender,
                PricePerHour = model.PricePerHour,
                PetSpecialty = model.PetSpecialty,
                CertificateUrl = model.CertificateUrl,
                Address = new Address
                {
                    Country = model.Country,
                    City = model.City,
                    Street = model.Street
                }
            };
            var result = await userManager.CreateAsync(doctor, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(doctor, "Doctor");
                return true;
            }
            return false;
        }

        public async Task<bool> CustomerRegister(CustomerRegisterDTO model)
        {
            var customer = new Customer
            {
                FName = model.FName,
                LName = model.LName,
                Email = model.Email,
                UserName = model.Email,
                ImgUrl = model.ImageUrl,
                Gender = model.Gender,
                Address = new Address
                {
                    Country = model.Country,
                    City = model.City,
                    Street = model.Street
                }
            };
            var result = await userManager.CreateAsync(customer, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(customer, "Customer");
                return true;
            }
            return false;
        }


      








    }
}

