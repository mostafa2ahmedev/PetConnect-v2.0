using Microsoft.AspNetCore.Http;
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
        private readonly IFaceComparisonService faceComparisonService;

        public AccountService(IUnitOfWork _unitOfWork,
            UserManager<ApplicationUser> _userManager,
            RoleManager<ApplicationRole> _roleManager,
            SignInManager<ApplicationUser> _signinManager,
            IAttachmentService _attachmentService,IJwtService _jwtService , IFaceComparisonService _faceComparisonService)
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
            roleManager = _roleManager;
            signinManager = _signinManager;
            attachmentService = _attachmentService;
            jwtService = _jwtService;
            faceComparisonService = _faceComparisonService;
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
                return user; 
            }
            else
                return null; 
        }



        public async Task<RegistrationResult> DoctorRegister(DoctorRegisterDTO registerDTO)
        {
            var response = new RegistrationResult();

            
            if (registerDTO.ProfileImage == null || registerDTO.IdCardImage == null || registerDTO.Certificate == null)
            {
                response.Errors.Add("Profile image, ID card, and Certificate are all required.");
                return response;
            }

            
            bool facesMatch = await faceComparisonService.AreFacesMatchingAsync(
                registerDTO.ProfileImage.OpenReadStream(),
                registerDTO.IdCardImage.OpenReadStream()
            );

            if (!facesMatch)
            {
                response.Errors.Add("Face verification failed. The images do not appear to be of the same person.");
                return response;
            }

            
            string imageName = await attachmentService.UploadAsync(registerDTO.ProfileImage, "img/doctors");
            string certificateName = await attachmentService.UploadAsync(registerDTO.Certificate, "img/certificates");

            var doctor = new Doctor
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                FName = registerDTO.FName,
                LName = registerDTO.LName,
                Gender = registerDTO.Gender,
                PetSpecialty = registerDTO.PetSpecialty,
                PhoneNumber = registerDTO.PhoneNumber,
                IsApproved = false,
                PricePerHour = registerDTO.PricePerHour,
                Address = new Address
                {
                    City = registerDTO.City,
                    Country = registerDTO.Country,
                    Street = registerDTO.Street
                },
         
                ImgUrl = $"/assets/img/doctors/{imageName}",
                CertificateUrl = $"/assets/img/certificates/{certificateName}"
            };

         
            var result = await userManager.CreateAsync(doctor, registerDTO.Password);


            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(doctor, "Doctor");

                response.Succeeded = true;
                response.UserId = doctor.Id;
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

