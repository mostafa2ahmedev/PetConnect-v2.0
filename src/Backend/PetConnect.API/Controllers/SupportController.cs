using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTOs.Blog;
using PetConnect.BLL.Services.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.BLL.Services.DTOs.Support.Admin;
using PetConnect.BLL.Services.DTOs.Support.User;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ISupportRequestService supportRequestService;
        private readonly IAdminSupportResponseService adminsupportResponseService;
        private readonly IFollowUpSupportRequestService _followUpSupportRequestService;

        public SupportController(ISupportRequestService supportRequestService, IAdminSupportResponseService supportResponseService
            ,IFollowUpSupportRequestService followUpSupportRequestService)
        {
            this.supportRequestService = supportRequestService;
            adminsupportResponseService = supportResponseService;
            _followUpSupportRequestService = followUpSupportRequestService;
        }

        [HttpPost(template: "CreateSupportRequest")]
        [EndpointSummary("Create a Support Request")]
        [Authorize]
        public async Task<ActionResult> CreateSupportRequest([FromForm] CreateSupportRequestDto supportRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponse(400, "Data is not correct"));

            }
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result =await supportRequestService.CreateSupportRequest(UserId!, supportRequestDto);
            if (result)
                return Ok(new GeneralResponse(200, "Support Request Created Successfully"));
            else
                return BadRequest("Error");

        }
        [HttpPost(template: "CreateFollowUpSupportRequest")]
        [EndpointSummary("Create a Follow Up Support Request")]
        [Authorize]
        public async Task<ActionResult> CreateFollowUpSupportRequest([FromForm] CreateFollowUpSupportRequestDto followUpSupportRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponse(400, "Data is not correct"));

            }
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _followUpSupportRequestService.CreateFollowUpSupportRequest(UserId!, followUpSupportRequestDto);
            if (result)
                return Ok(new GeneralResponse(200, "Follow Up Support Request Created Successfully"));
            else
                return BadRequest("Error");

        }

        [HttpPost(template: "CreateSupportResponse")]
        [EndpointSummary("Create an Admin Support Response")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> CreateAdminSupportResponse([FromForm] CreateAdminSupportResponseDto supportRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponse(400, "Data is not correct"));

            }

            var result = await adminsupportResponseService.CreateSupportResponse(supportRequestDto);
            if (result)
                return Ok(new GeneralResponse(200, "Support Response Created Successfully"));
            else
                return BadRequest("Error");

        }


        [HttpGet("SupportRequests")]
        [ProducesResponseType(typeof(List<AdminSupportRequestDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get Support Requests that need action From Admin")]
        [Authorize(Roles = "Admin")]

        public ActionResult GetAdminSupportRequests()
        {
            var result = supportRequestService.GetAdminSupportRequests();
            return Ok(new GeneralResponse(200, result));
        }








        [HttpGet("UserSubmittedRequestsInfo")]
        [ProducesResponseType(typeof(List<SubmittedSupportRequestDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get User Submitted Requests Info")]
        [Authorize]

        public ActionResult GetUserSubmittedRequestsInfo()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = supportRequestService.GetSubmittedSupportRequestsForUser(UserId!);
            return Ok(new GeneralResponse(200, result));
        }

        [HttpGet("UserSubmittedRequestsDetails")]
        [ProducesResponseType(typeof(List<SubmittedSupportRequestDetailsDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get User Submitted Requests Details")]
        [Authorize]

        public ActionResult GetUserSubmittedRequestsDetails([FromQuery] int supportRequestId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Role = User.FindFirstValue(ClaimTypes.Role);
            var result = supportRequestService.GetSubmittedSupportRequestsDetails(Role!,UserId!, supportRequestId);
            return Ok(new GeneralResponse(200, result));
        }
        [HttpPut("UpdateRequestStatusPriority")]
        [EndpointSummary("Update Request Status Priority")]
        [Authorize(Roles ="Admin")]
        public ActionResult UpdateRequestStatusPriority([FromBody] UpdatePriorityStatusDto updatePriorityStatusDto )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponse(400, "Data is not correct"));

            }
            var result = adminsupportResponseService.UpdatePriorityStatus(updatePriorityStatusDto);
            if (result)
                return Ok(new GeneralResponse(200, "Priority Updated Successfully"));
            else
                return BadRequest("Error");
        }


    }
}