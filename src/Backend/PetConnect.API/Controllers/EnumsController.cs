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
        [EndpointSummary("Get Pet Status")]
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
        [EndpointSummary("Get OwnerShip Status")]
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




      
        [HttpGet("order-product-statuses")]
        [EndpointSummary("Get Order Product Status")]
        public IActionResult GetOrderProductStatuses()
        {
            var values = Enum.GetValues(typeof(OrderProductStatus))
                .Cast<OrderProductStatus>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }

        
        [HttpGet("order-statuses")]
        [EndpointSummary("Get Order Status")]
        public IActionResult GetOrderStatuses()
        {
            var values = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }

      
        [HttpGet("blog-topics")]
        [EndpointSummary("Get Blog Topics")]
        public IActionResult GetBlogTopics()
        {
            var values = Enum.GetValues(typeof(BlogTopic))
                .Cast<BlogTopic>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }

  
        [HttpGet("blog-types")]
        [EndpointSummary("Get Blog Types")]
        public IActionResult GetBlogTypes()
        {
            var values = Enum.GetValues(typeof(BlogType))
                .Cast<BlogType>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }

        [HttpGet("SupportRequest-types")]
        [EndpointSummary("Get Support Request Types")]
        public IActionResult GetSupportRequestTypes()
        {
            var values = Enum.GetValues(typeof(SupportRequestType))
                .Cast<SupportRequestType>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }
        [HttpGet("SupportRequest-Status")]
        [EndpointSummary("Get Support Request Status")]
        public IActionResult GetSupportRequestStatus()
        {
            var values = Enum.GetValues(typeof(SupportRequestStatus))
                .Cast<SupportRequestStatus>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }
        [HttpGet("SupportRequest-Priority")]
        [EndpointSummary("Get Support Request Priority")]
        public IActionResult GetSupportRequestPriority()
        {
            var values = Enum.GetValues(typeof(SupportRequestPriority))
                .Cast<SupportRequestPriority>()
                .Select(e => new
                {
                    key = (int)e,
                    value = e.ToString()
                });

            return Ok(values);
        }
    }
}
