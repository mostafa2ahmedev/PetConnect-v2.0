using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.PetDto;
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



        [HttpGet()]
        [ProducesResponseType(typeof(List<CustomerDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Customers")]
        //[Authorize(Roles = "Admin")]
        public ActionResult GetAll()
        {
            var Customers = _customerService.GetAllCustomers();
            return Ok(new GeneralResponse(200, Customers));
        }



        [HttpGet("Profile")]
        [ProducesResponseType(typeof(List<CustomerDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get Customer Profile")]
        [Authorize(Roles = "Customer")]

        public ActionResult GetCustomerProfile()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Customer = _customerService.GetProfile(customerId!);
            return Ok(new GeneralResponse(200, Customer));
        }




        [HttpPut("UpdateProfile")]
        [EndpointSummary("Update Customer Profile")]
        [Authorize(Roles = "Customer")]
        public  async Task<ActionResult> UpdateCustomerProfile([FromForm] UpdateCustomerProfileDTO CustomerProfileDTO)
        {
            if (!ModelState.IsValid) {
                    var errors = ModelState
                        .Where(ms => ms.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    return BadRequest(new GeneralResponse(400, errors));
                }
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _customerService.UpdateProfile(CustomerProfileDTO, customerId!);
            if (result > 0)
                return Ok(new GeneralResponse(200, "Profile Updated Successfully"));
            else
                return NotFound(new GeneralResponse(404, "Error Happened"));


        }

        [HttpDelete("DeleteProfile/{CustomerId}")]
        [EndpointSummary("Delete Customer Profile")]
        //[Authorize(Roles = "Admin")]

        public ActionResult DeleteProfile(string CustomerId) {


            if (CustomerId == null)
                return BadRequest(new GeneralResponse(400, "Customer ID can't be null"));
            if (_customerService.Delete(CustomerId) == 0)
            {
                return NotFound(new GeneralResponse(404, $"No Customer found with ID = {CustomerId}"));
            }

            return Ok(new GeneralResponse(200, "Profile deleted successfully"));

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

        
        [HttpGet("CusReqAdoptions")]
        [EndpointSummary("Get Sent Adoption Requests")]
        [Authorize(Roles = "Customer")]
        public ActionResult GetSentAdoptionRequests()
        {

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result=    _customerService.GetCustomerReqAdoptionsPendingData(customerId!);

            return Ok(new GeneralResponse(200, result));
        }
        [HttpGet("CusRecAdoptions")]
        [EndpointSummary("Get Received Adoption Requests")]
        [Authorize(Roles = "Customer")]
        public ActionResult GetReceivedAdoptionRequests()
        {

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _customerService.GetCustomerRecAdoptionsPendingData(customerId!);

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
            var result = _customerService.GetCustomerOwnedPets(customerId!);

            return Ok(new GeneralResponse(statusCode: 200, result));

        }
    }
}
