using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }



        [HttpPost]
        [EndpointSummary("Submit An Adoption Request")]
        [Authorize(Roles = "Customer")]
        public ActionResult RequestAdoption([FromBody] CusRequestAdoptionDto cusRequestAdoptionDto) {

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

                return BadRequest(new GeneralResponse(400, errors));
            }
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _customerService.RequestAdoption(cusRequestAdoptionDto, customerId!);

            return Ok(new GeneralResponse(200, "Request Registered Successfully"));
        }


        [HttpGet]
        [EndpointSummary("Get Customer Adoption Requests")]
        [Authorize(Roles = "Customer")]
        public ActionResult GetCustomerAdoptionRequests()
        {

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result=    _customerService.GetCustomerReqAdoptionsPendingData(customerId!);

            return Ok(new GeneralResponse(200, result));
        }




        [HttpPut("ApproveORCancel")]
        [EndpointSummary("Approve OR Cancel Request For Customer")]
        [Authorize(Roles = "Customer")]
        public ActionResult ApproveORCancelCustomerRequest([FromBody]ApproveORCancelCustomerRequest approveORCancelCustomerRequestDTO)
        {

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result=  _customerService.ApproveOrCancelCustomerAdoptionRequest(approveORCancelCustomerRequestDTO,customerId!);

            if (result == null)
                return BadRequest(new GeneralResponse(400, "Error in submitted data"));
            else if (result == AdoptionStatus.Approved.ToString())
                return Ok(new GeneralResponse(statusCode: 200, "Request Approved Successfully"));
            else
                return Ok(new GeneralResponse(statusCode: 200, "Request Canceled Successfully"));
        }

        [HttpGet("CustomerPets")]
        [EndpointSummary("Get Customer Owned Pets")]
        [Authorize(Roles = "Customer")]
        public ActionResult GetCustomerOwnedPets()
        {

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _customerService.GetCustomerOwnedPetsForCustomer(customerId!);

            return Ok(new GeneralResponse(statusCode: 200, result));

        }
    }
}
