using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Interfaces;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService _adminService)
        {
            adminService = _adminService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var dashboard = adminService.GetPendingDoctorsAndPets();
            return Ok(dashboard);
        }
        [HttpPost("approve-doctor/{id}")]
        public IActionResult ApproveDoctor(string id)
        {
            try
            {
                adminService.ApproveDoctor(id);
                var dashboard = adminService.GetPendingDoctorsAndPets();
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return (StatusCode(500, new { message = "Error Approving Doctor", error = ex.Message }));
            }
            
        }
        [HttpPost("approve-pet/{id}")]
        public IActionResult ApprovePet(int id)
        {
            try
            {
                adminService.ApprovePet(id);
                var dashboard = adminService.GetPendingDoctorsAndPets();
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return (StatusCode(500, new {message = "Error approving pet" , error = ex.Message }));
            }
            
        }
    }
}
