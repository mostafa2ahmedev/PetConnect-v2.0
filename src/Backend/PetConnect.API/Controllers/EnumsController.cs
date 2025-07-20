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
    }
}
