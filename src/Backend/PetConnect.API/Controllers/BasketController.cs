using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Basket;
using PetConnect.BLL.Services.Interfaces;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;

        public BasketController(IBasketService basketService)
        {
            this.basketService = basketService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerBasketDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get Customer Basket ")]
        public async Task<ActionResult<CustomerBasketDto>> GetCustomerBasketAsync(string basketId) {
            var basket = await basketService.GetCustomerBasketAsync(basketId);
            return Ok(basket);
        }
        [HttpPost]
        [ProducesResponseType(typeof(List<CustomerBasketDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Create Or Update Baskets")]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basketDto)
        {
            var basket = await basketService.UpdateCustomerBasketAsync(basketDto);
            return Ok(basket);
        }
        [HttpDelete]

        [EndpointSummary("Delete basket by id")]
        public async Task DeleteBasket(string id)
            {
          await basketService.DeleteCustomerBasketAsync(id);
            
        }


    }
}
