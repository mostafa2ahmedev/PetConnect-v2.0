using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/reviews/doctor/{doctorId}
        [HttpGet("doctor/{doctorId}")]
        public ActionResult<IEnumerable<Review>> GetByDoctorId(string doctorId)
        {
            var reviews = _reviewService.GetByDoctorId(doctorId);
            return Ok(reviews);
        }

        // GET: api/reviews/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public ActionResult<IEnumerable<Review>> GetByCustomerId(string customerId)
        {
            var reviews = _reviewService.GetByCustomerId(customerId);
            return Ok(reviews);
        }

        // POST: api/reviews
        [HttpPost]
        public ActionResult<Review> AddReview([FromBody] Review review)
        {
            if (review == null)
                return BadRequest("Review object is null");

            var createdReview = _reviewService.AddReview(review);
            return CreatedAtAction(nameof(GetByDoctorId), new { doctorId = createdReview.DoctorId }, createdReview);
        }

        // DELETE: api/reviews/{reviewId}
        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(Guid reviewId)
        {
            var result = _reviewService.DeleteReview(reviewId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
