using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTOs.Seller;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.Interfaces;
using System.Security.Claims;
using PetConnect.BLL.Services.DTOs.OrderProduct;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private readonly IOrderProductService _orderProductService;

        public OrderProductController(IOrderProductService orderProductService)
        {
            _orderProductService = orderProductService;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(List<OrderProductForSellerConfirmationDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get OrderProducts For Seller Confirmation   (Approve OR Deny OrderProduct Status)")]
        [Authorize(Roles = "Seller")]
        public ActionResult GetOrderProductForSellerConfirmation()
        {
            var SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _orderProductService.GetOrderProductForSellerConfirmationDto(SellerId!);
          
            return Ok(new GeneralResponse(200, result));
        }

        [HttpPost("Action")]
        [EndpointSummary("Shipp Or Deny OrderProduct In Order For Customer")]
        [Authorize(Roles = "Seller")]
        public ActionResult ShippOrDenyingOrderProductInOrder([FromBody] ShipOrDenyOrderProductDto shipOrDenyOrderProductDto)
        {
            var SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           var result=  _orderProductService.ShippingOrDenyingOrderProductInOrder(SellerId!,shipOrDenyOrderProductDto);

            if (result == 0 )
                return BadRequest(new GeneralResponse(400, "Error Happened"));
            else if(result==null)
                return NotFound(new GeneralResponse(404, "Order Product Not Found"));

            return Ok(new GeneralResponse(200, "OrderProduct Status Changed Successfully"));
        }
    }
}
