using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Review;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.UnitofWork;
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
        public IActionResult AddReview([FromBody] ReviewCreatedByCustToDocDTO dto)
        {
            if (dto == null)
                return BadRequest(new GeneralResponse(400,"Review data is required."));

            var review = _reviewService.AddCustomerReview(dto);
            return Ok(new GeneralResponse(201,"Created Successfully"));
            //return CreatedAtAction(nameof(GetByCustomerId), new { customerId = review.CustomerId }, review);
        }

        //public IActionResult AddReview([FromBody] ReviewCreateDto dto)
        //{
        //    if (dto == null)
        //        return BadRequest("Review data is required.");

        //    var review = _reviewService.AddReview(dto);
        //    return CreatedAtAction(nameof(GetByCustomerId), new { customerId = review.CustomerId }, review);
        //}

        [HttpGet("by-customer/{customerId}")]
        public IActionResult GetByCustomerId(string customerId)
        {
            //var reviews = _reviewService.GetByCustomerId(customerId);
            var reviews = _reviewService.GetCustomer(customerId);
            if (reviews.Count() == 0)
                return NotFound(new GeneralResponse(404, "No Reviews Found"));

            return Ok(new GeneralResponse(200, reviews));
        }

        [HttpGet("by-doctor/{doctorId}")]
        public IActionResult GetByDoctorId(string doctorId)
        {
            //var reviews = _reviewService.GetByDoctorId(doctorId);
            var reviews = _reviewService.GetDoctor(doctorId);
            if (reviews.Count() == 0)
                return NotFound(new GeneralResponse(404,"No Reviews Found"));
            
            return Ok(new GeneralResponse(200,reviews));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var deleted = _reviewService.DeleteReview(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPost("IsReviewable")]
        public IActionResult CanBeReviewed([FromBody] IsReviewableDTO isReviewable)
        {
           var result = _reviewService.AnyByAppointment(isReviewable.AppointmentId);
            if (result)
                return BadRequest(new GeneralResponse(400,"Already Written a Review"));

            return Ok(new GeneralResponse(200,result));
        }
    }
}
