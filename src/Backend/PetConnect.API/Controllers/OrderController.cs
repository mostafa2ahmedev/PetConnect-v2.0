using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTOs.Order;
using PetConnect.BLL.Services.Interfaces;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddedOrderDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await orderService.AddOrder(dto);
            if (result > 0)
                return Ok("Order added successfully");

            return BadRequest("Failed to add order");
        }

        // GET: api/Order
        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = orderService.GetAllOrders();
            return Ok(orders);
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = orderService.GetOrderDetails(id);
            if (order == null)
                return NotFound("Order not found");

            return Ok(order);
        }

        // PUT: api/Order/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatedOrderDTO dto)
        {
            if (id != dto.ID)
                return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await orderService.UpdateOrder(dto);
            if (result > 0)
                return Ok("Order updated successfully");

            return NotFound("Order not found or update failed.");
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = orderService.DeleteOrder(id);
            if (result > 0)
                return Ok("Order deleted successfully");

            return NotFound("Order not found");
        }
    }
}
    

