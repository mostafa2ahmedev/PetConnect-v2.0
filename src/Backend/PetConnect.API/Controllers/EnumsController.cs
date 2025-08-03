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

        // ✅ OrderStatus
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
    }
}
