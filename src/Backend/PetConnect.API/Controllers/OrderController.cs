using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTOs.Basket;
using PetConnect.BLL.Services.DTOs.Order;
using PetConnect.BLL.Services.Interfaces;
using System.Security.Claims;

namespace PetConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Order
        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _orderService.GetAllOrders();
            return Ok(orders);
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _orderService.GetOrderDetails(id);
            if (order == null)
                return NotFound($"No order found with ID {id}");

            return Ok(order);
        }

        // POST: api/Order
        [HttpPost]
        public IActionResult Add([FromBody] AddedOrderDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderId = _orderService.AddOrder(dto);
            return Ok(new { OrderId = orderId });
        }
        [HttpPost("legacyCode")]
        public async Task<IActionResult> CreateOrder(OrderToCreateDto OrderToCreateDto)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerEmail = User.FindFirstValue(ClaimTypes.Email);
            var reuslt = await _orderService.CreateOrderAsync( customerId!, customerEmail!, OrderToCreateDto);
            return Ok(reuslt);
        }
        // PUT: api/Order
        [HttpPut]
        public IActionResult Update([FromBody] UpdatedOrderDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _orderService.UpdateOrder(dto);
            if (result == 0)
                return NotFound($"No order found with ID {dto.ID}");

            return Ok($"Order {dto.ID} updated successfully.");
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _orderService.DeleteOrder(id);
            if (result == 0)
                return NotFound(new { message = $"No order found with ID {id}" });

            return Ok(new { message = $"Order {id} deleted successfully." });
        }


        [HttpGet("customer/{customerId}")]
        public IActionResult GetOrdersByCustomer(string customerId)
        {
            var result = _orderService.GetOrdersByCustomer(customerId);
            return Ok(result);
        }

    }
}
