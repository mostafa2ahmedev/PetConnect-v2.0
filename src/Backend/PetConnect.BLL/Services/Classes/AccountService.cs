using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTO.Account;
using PetConnect.BLL.Services.DTOs.Account;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.BLL.Services.Models;
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
        private readonly IAttachmentService attachmentService;
        private readonly IJwtService jwtService;

        public AccountService(IUnitOfWork _unitOfWork,
            UserManager<ApplicationUser> _userManager,
            RoleManager<ApplicationRole> _roleManager,
            SignInManager<ApplicationUser> _signinManager,
            IAttachmentService _attachmentService,IJwtService _jwtService)
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
            roleManager = _roleManager;
            signinManager = _signinManager;
            attachmentService = _attachmentService;
            jwtService = _jwtService;
        }
        public async Task<List<string>> GetAllRolesAsync()
        {
            return await roleManager.Roles.Select(r => r.Name).ToListAsync();
        }

        public async Task<ApplicationUser?> SignIn(SignInDTO model)
        {
            var result = await signinManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                return user; // need to be seeeen 
            }
            else
                return null; // need to be seeeen 
        }

        public async Task<RegistrationResult> DoctorRegister(DoctorRegisterDTO registerDTO)
        {
            var doctor = new Doctor
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                FName = registerDTO.FName,
                LName = registerDTO.LName,
                Gender = registerDTO.Gender,
                PhoneNumber = registerDTO.PhoneNumber,
                IsApproved = false,
                CertificateUrl = "",
                PricePerHour = registerDTO.PricePerHour,
                PetSpecialty = registerDTO.PetSpecialty,
                Address = new Address
                {
                    City = registerDTO.City,
                    Country = registerDTO.Country,
                    Street = registerDTO.Street
                }
            };

            var result = await userManager.CreateAsync(doctor, registerDTO.Password);
            var response = new RegistrationResult();

            if (result.Succeeded)
            {
                string? imageName;
                string certificateName;

                if (registerDTO.Image != null && registerDTO.Certificate != null )
                {
                    imageName = await attachmentService.UploadAsync(registerDTO.Image, "img/doctors");
                    certificateName = await attachmentService.UploadAsync(registerDTO.Certificate, "img/certificates");
                    doctor.CertificateUrl = $"/assets/img/certificates/{certificateName}";

                }
                else
                {
                    imageName = registerDTO.Gender == DAL.Data.Enums.Gender.Male ? "0baf3fbe-4277-4199-aaaa-ba3e396c8c43.png" : "29b209e7-32aa-4669-8209-c113b968b527.png";
                }

                doctor.ImgUrl = $"/assets/img/doctors/{imageName}";

                await userManager.UpdateAsync(doctor);
                await userManager.AddToRoleAsync(doctor, "Doctor");
                //await signinManager.SignInAsync(doctor, isPersistent: false);

                response.Succeeded = true;
            }
            else
            {
                response.Succeeded = false;
                response.Errors = result.Errors.Select(e => e.Description).ToList();
            }

            return response;
        }


        public async Task<RegistrationResult> CustomerRegister(CustomerRegisterDTO registerDTO)
        {
            var customer = new Customer
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                FName = registerDTO.FName,
                LName = registerDTO.LName,
                Gender = registerDTO.Gender,
                PhoneNumber = registerDTO.PhoneNumber,
                IsApproved = true,
                Address = new Address
                {
                    City = registerDTO.City,
                    Country = registerDTO.Country,
                    Street = registerDTO.Street
                }
            };

            var result = await userManager.CreateAsync(customer, registerDTO.Password);

            var response = new RegistrationResult();

            if (result.Succeeded)
            {
                string? fileName;

                if (registerDTO.Image != null)
                {
                    fileName = await attachmentService.UploadAsync(registerDTO.Image, "img/person");
                }
                else
                {
                    fileName = registerDTO.Gender == DAL.Data.Enums.Gender.Male ? "default-m.png" : "default-f.png";
                }

                customer.ImgUrl = $"/assets/img/person/{fileName}";

                await userManager.UpdateAsync(customer);
                await userManager.AddToRoleAsync(customer, "Customer");
                //await signinManager.SignInAsync(customer, isPersistent: false);

                response.Succeeded = true;
            }
            else
            {
                response.Succeeded = false;
                response.Errors = result.Errors.Select(e => e.Description).ToList();
            }

            return response;
        }

        public async Task<RegistrationResult> SellerRegister(SellerRegisterDto registerDTO)
        {

            var Seller = new Seller
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                FName = registerDTO.FName,
                LName = registerDTO.LName,
                Gender = registerDTO.Gender,
                PhoneNumber = registerDTO.PhoneNumber,
                IsApproved = true,
                Address = new Address
                {
                    City = registerDTO.City,
                    Country = registerDTO.Country,
                    Street = registerDTO.Street
                }
            };
   
            var result = await userManager.CreateAsync(Seller, registerDTO.Password);

            var response = new RegistrationResult();

            if (result.Succeeded)
            {
                string? fileName;

                if (registerDTO.Image != null)
                {
                    fileName = await attachmentService.UploadAsync(registerDTO.Image, "img/person");
                }
                else
                {
                    fileName = registerDTO.Gender == DAL.Data.Enums.Gender.Male ? "default-m.png" : "default-f.png";
                }

                Seller.ImgUrl = $"/assets/img/person/{fileName}";

                await userManager.UpdateAsync(Seller);
                await userManager.AddToRoleAsync(Seller, "Seller");
                //await signinManager.SignInAsync(customer, isPersistent: false);

                response.Succeeded = true;
            }
            else
            {
                response.Succeeded = false;
                response.Errors = result.Errors.Select(e => e.Description).ToList();
            }

            return response;
        }




    }
}

