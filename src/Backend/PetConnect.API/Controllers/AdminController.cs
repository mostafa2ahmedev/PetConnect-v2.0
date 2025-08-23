using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Admin;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.BLL.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService _adminService)
        {
            adminService = _adminService;
        }



        [HttpGet]
        [EndpointSummary("Get All Pending Doctors And Pets")]
        [ProducesResponseType(typeof(List<AdminDashboardDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Index()
        {
            var dashboard = adminService.GetPendingDoctorsAndPets();
            return Ok(new GeneralResponse(200, dashboard));
        }


        [HttpPut(template: "doctors/{id}/approve")]
        [EndpointSummary("Approve Doctor By Id")]
        [ProducesResponseType(typeof(DoctorDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ApproveDoctor(string id)
        {
            var result = adminService.ApproveDoctor(id);
            if (result == null)
                return NotFound();
            else
            {
                return Ok(new GeneralResponse(200, result));
            }
        }


        [HttpPut("doctors/{id}/reject")]
        [EndpointSummary("Reject Doctor By Id")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DoctorDetailsDTO), StatusCodes.Status200OK)]
        public IActionResult RejectDoctor(string id, [FromBody] string message)
        {
            var result = adminService.RejectDoctor(id, message);
            if (result == null)
                return NotFound();
            else
            {
                return Ok(new GeneralResponse(200, result));
            }
        }



        [HttpPut("pets/{id}/approve")]
        [EndpointSummary("Approve Pet By Id")]
        [ProducesResponseType(typeof(PetDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ApprovePet(int id)
        {
            var result = adminService.ApprovePet(id);
            if (result == null)
                return NotFound();
            else
            {
                return Ok(new GeneralResponse(200, result));
            }
        }



        [HttpPut("pets/{id}/reject")]
        [EndpointSummary("Reject Pet By Id")]
        [ProducesResponseType(typeof(PetDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RejectPet(int id, [FromBody] string message)
        {
            var result = adminService.RejectPet(id, message);
            if (result == null)
                return NotFound();
            else
            {
                return Ok(new GeneralResponse(200, result));
            }
        }


        [HttpGet("statistics")]
        [EndpointSummary("Get overall statistics for pets, users, doctors, and customers")]
        [ProducesResponseType(typeof(AdminStatisticsDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatistics()
        {
            var stats = await adminService.GetAdminStatistics();
            return Ok(new GeneralResponse(200, stats));
        }


        [HttpGet("Profile")]
        [ProducesResponseType(typeof(List<CustomerDetailsDTO>), StatusCodes.Status200OK)]
        [EndpointSummary("Get Customer Profile")]
        [Authorize(Roles = "Admin")]

        public ActionResult GetCustomerProfile()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Customer = adminService.GetProfile(customerId!);
            return Ok(new GeneralResponse(200, Customer));
        }

    }



}
