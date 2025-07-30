using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumsController : ControllerBase
    {
        [HttpGet("pet-status-values")]
        public IActionResult GetPetStatuses()
        {
            var values = Enum.GetValues(typeof(PetStatus))
                .Cast<PetStatus>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }

        [HttpGet("ownership-types")]
        public IActionResult GetOwnershipTypes()
        {
            var values = Enum.GetValues(typeof(Ownership))
                .Cast<Ownership>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }

        [HttpGet("notification-types")]
        public IActionResult GetNotificationsTypes()
        {
            var values = Enum.GetValues(typeof(NotificationType))
                .Cast<NotificationType>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }

        [HttpGet("genders")]
        public IActionResult GetGenders()
        {
            var values = Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }

        [HttpGet("user-message-type")]
        public IActionResult GetUserMessageTypes()
        {
            var values = Enum.GetValues(typeof(UserMessageType))
                .Cast<UserMessageType>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }

        [HttpGet("appointment-status")]
        public IActionResult GetAppointMentStatus()
        {
            var values = Enum.GetValues(typeof(AppointmentStatus))
                .Cast<AppointmentStatus>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }

        [HttpGet("admin-message-type")]
        public IActionResult GetAdminMessageType()
        {
            var values = Enum.GetValues(typeof(AdminMessageType))
                .Cast<AdminMessageType>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }


    }
}
