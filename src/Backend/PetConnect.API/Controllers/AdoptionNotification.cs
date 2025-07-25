using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Notification.AdoptionNotification;
using PetConnect.BLL.Services.Interfaces;
using System.Security.Claims;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionNotification : ControllerBase
    {
        private readonly IAdoptionNotificationService _adoptionNotificationService;

        public AdoptionNotification(IAdoptionNotificationService adoptionNotificationService)
        {
            _adoptionNotificationService = adoptionNotificationService;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(List<AdoptionNotificationData>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Adoption Notification To Signed User ")]
        [Authorize(Roles = "Customer")]
        public ActionResult GetAllNotification()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Notifications = _adoptionNotificationService.GetAllAdoptionNotifications(customerId!);
            return Ok(new GeneralResponse(200, Notifications));
        }
    }
}
