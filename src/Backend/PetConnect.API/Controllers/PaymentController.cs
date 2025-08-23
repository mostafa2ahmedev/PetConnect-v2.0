using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTOs.Basket;
using PetConnect.BLL.Services.Interfaces;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        [Authorize]
        [HttpPost("{BasketId}")]

        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string BasketId , int deliveryMethodId)
        {

            var Basket = await paymentService.CreateOrUpdatePaymentIntentAsync(BasketId, deliveryMethodId);
            return Ok(Basket);
        }
    }
}
