using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTOs.Notification;
using PetConnect.BLL.Services.Interfaces;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService _notificationService)
        {
            notificationService = _notificationService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<NotificationDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Get All Notifications For User By User Id")]
        public IActionResult GetAllNotificationByUserId(string id)
        {
            var notification = notificationService.GetAllNotificationsByUserId(id);
                return Ok(notification);
        }

        [HttpPut("{id}/read")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [EndpointSummary("Mark An Exiting Notification As Read By Notification Id")]
        public IActionResult MarkNotificationAsRead(Guid id)
        {
            var res = notificationService.MarkNotificationAsRead(id);
            if (res)
                return Ok("Notification Marked as Read");
            return NotFound($"No Notification Found With {id}");
        }

        [HttpDelete("{id}/delete")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [EndpointSummary("Delete an Exiting Notification By Notification Id")]

        public IActionResult DeleteNotification(Guid id)
        {
            var res = notificationService.DeleteNotification(id);
            if (res)
                return Ok("Notification Deleted");
            return NotFound($"No Notification Found With {id}");
        }


    }
}
