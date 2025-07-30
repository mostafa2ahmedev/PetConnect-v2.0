using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTO.Account;
using PetConnect.BLL.Services.DTOs.Account;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.BLL.Services.Models;
using PetConnect.DAL.Data.Identity;
using PetConnect.DAL.Data.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IJwtService jwtService;

        public AccountController(IAccountService _accountService,
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager, RoleManager<ApplicationRole> _roleManager, IJwtService _jwtService)
        {
            accountService = _accountService;
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            jwtService = _jwtService;
        }

        [HttpPost("register/customer")]
        [EndpointSummary("Register a new customer.")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCustomerRegister([FromForm] CustomerRegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors as array
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return BadRequest(new
                {
                    success = false,
                    errors = errors
                });
            }

            RegistrationResult result = await accountService.CustomerRegister(registerDTO);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    success = true,
                    message = "Customer Registered Successfully."
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = result.Errors.ToArray()
            });
        }
        [HttpPost("register/doctor")]
        [EndpointSummary("Register a new doctor.")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostDoctorRegister([FromForm] DoctorRegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors as array
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return BadRequest(new
                {
                    success = false,
                    errors = errors
                });
            }

            RegistrationResult result = await accountService.DoctorRegister(registerDTO);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    success = true,
                    message = "Doctor Registered Successfully."
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    errors = result.Errors.ToArray()
                });
            }
        }

        [HttpPost("login")]
        [EndpointSummary("Login with email and password.")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostLogin(SignInDTO signInDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }

            ApplicationUser? user = await accountService.SignIn(signInDTO);

            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            var authResponse = await jwtService.CreateJwtToken(user);

            return Ok(authResponse);
        }

    }
}
