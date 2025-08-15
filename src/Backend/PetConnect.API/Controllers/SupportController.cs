using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTOs.Blog;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Support;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.BLL.Services.DTOs.Customer;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ISupportRequestService supportRequestService;
        private readonly ISupportResponseService supportResponseService;

        public SupportController(ISupportRequestService supportRequestService, ISupportResponseService supportResponseService)
        {
            this.supportRequestService = supportRequestService;
            this.supportResponseService = supportResponseService;
        }

        [HttpPost(template: "CreateSupportRequest")]
        [EndpointSummary("Create a Support Request")]
        [Authorize]
        public ActionResult CreateSupportRequest([FromBody]CreateSupportRequestDto supportRequestDto)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = supportRequestService.CreateSupportRequest(UserId!, supportRequestDto);
            if (result)
                return Ok(new GeneralResponse(200, "Support Request Created Successfully"));
            else
                return BadRequest("Error");

        }

        [HttpGet("SupportRequests")]
        [ProducesResponseType(typeof(List<SupportRequestDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get Support Requests that need action From Admin")]


        public ActionResult GetSupportRequests()
        {
            var result = supportRequestService.GetSupportRequests();
            return Ok(new GeneralResponse(200, result));
        }



        [HttpPost(template: "CreateSupportResponse")]
        [EndpointSummary("Create a Support Response")]

        public ActionResult CreateSupportResponse(CreateSupportResponseDto supportRequestDto)
        {


            var result = supportResponseService.CreateSupportResponse(supportRequestDto);
            if (result)
                return Ok(new GeneralResponse(200, "Support Response Created Successfully"));
            else
                return BadRequest("Error");

        }
    }
}