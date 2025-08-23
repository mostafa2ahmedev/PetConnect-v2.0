using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.Interfaces;
using System.Security.Claims;
using PetConnect.BLL.Services.DTOs.Seller;
using PetConnect.DAL.Data.Models;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpGet("Profile")]
        [ProducesResponseType(typeof(List<SellerDetailsDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get Seller Profile")]
        [Authorize(Roles = "Seller")]
        public ActionResult GetSellerProfile()
        {
            var SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Seller = _sellerService.GetProfile(SellerId!);
            if(Seller==null)
                return NotFound(new GeneralResponse(statusCode: 404, "Seller Not Found"));

            return Ok(new GeneralResponse(200, Seller));
        }
        [HttpGet()]
        [ProducesResponseType(typeof(List<SellerDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Sellers")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetAll()
        {
            var Sellers = _sellerService.GetAllSellers();
            return Ok(new GeneralResponse(200, Sellers));
        }



        [HttpPut("UpdateProfile")]
        [EndpointSummary("Update Seller Profile")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> UpdateSellerProfile([FromForm] UpdateSellerProfileDto SellerProfileDTO)
        {
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
            var SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _sellerService.UpdateProfile(SellerId!, SellerProfileDTO);
            if (result > 0)
                return Ok(new GeneralResponse(200, "Profile Updated Successfully"));
            else
                return NotFound(new GeneralResponse(404, "Error Happened"));


        }

        [HttpDelete("DeleteProfile/{SellerId}")]
        [EndpointSummary("Delete Seller Profile")]
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteProfile(string SellerId)
        {


            if (SellerId == null)
                return BadRequest(new GeneralResponse(400, "Customer ID can't be null"));
            if (_sellerService.Delete(SellerId) == 0)
            {
                return NotFound(new GeneralResponse(404, $"No Seller found with ID = {SellerId}"));
            }

            return Ok(new GeneralResponse(200, "Profile deleted successfully"));

        }

        [HttpGet("Products")]
        [ProducesResponseType(typeof(List<SellerDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Products for Current Seller")]
        [Authorize(Roles = "Seller")]
        public ActionResult GetAllProducts()
        {
            var SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var Seller = _sellerService.GetProfile(SellerId!);
            if (SellerId == null)
                return NotFound(new GeneralResponse(statusCode: 404, "Seller Not Found"));
            var SellerProducts = _sellerService.GetAllProducts(SellerId);
            return Ok(new GeneralResponse(200, SellerProducts));
        }

    }
}
