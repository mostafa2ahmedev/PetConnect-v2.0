using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTOs.Review;
using PetConnect.BLL.Services.Interfaces;
using System;

namespace PetConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public IActionResult AddReview([FromBody] ReviewCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Review data is required.");

            var review = _reviewService.AddReview(dto);
            return CreatedAtAction(nameof(GetByCustomerId), new { customerId = review.CustomerId }, review);
        }

        [HttpGet("by-customer/{customerId}")]
        public IActionResult GetByCustomerId(string customerId)
        {
            var reviews = _reviewService.GetByCustomerId(customerId);
            return Ok(reviews);
        }

        [HttpGet("by-doctor/{doctorId}")]
        public IActionResult GetByDoctorId(string doctorId)
        {
            var reviews = _reviewService.GetByDoctorId(doctorId);
            return Ok(reviews);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var deleted = _reviewService.DeleteReview(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
