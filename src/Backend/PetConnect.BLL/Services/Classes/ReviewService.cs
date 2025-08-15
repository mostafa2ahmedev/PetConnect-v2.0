using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Review;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetConnect.BLL.Services.Classes
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ReviewDto> GetByCustomerId(string customerId)
        {
            var reviews = _reviewRepository.GetByCustomerId(customerId);
            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Rating = r.Rating,
                ReviewText = r.ReviewText,
                ReviewDate = r.ReviewDate,
            });
        }

        public IEnumerable<ReviewDto> GetByDoctorId(string doctorId)
        {
            var reviews = _reviewRepository.GetByDoctorId(doctorId);
            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Rating = r.Rating,
                ReviewText = r.ReviewText,
                ReviewDate = r.ReviewDate,
            });
        }

        public bool AnyByAppointment(Guid appointmentId)
        {
            return _reviewRepository.AnyByAppointment(appointmentId);
        }

        public ReviewDto AddReview(ReviewCreateDto dto)
        {
            Review review = new Review
            {
                AppointmentId = dto.AppointmentId,
                Rating = dto.Rating,
                ReviewText = dto.ReviewText,
                ReviewDate = DateTime.UtcNow
            };

            _reviewRepository.Add(review);
            _unitOfWork.SaveChanges();

            return new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                ReviewDate = review.ReviewDate,
               
            };
        }

        public bool DeleteReview(int reviewId)
        {
            var review = _reviewRepository.GetByID(reviewId);
            if (review == null)
                return false;

            _reviewRepository.Delete(review);
            _unitOfWork.SaveChanges();
            return true;
        }
    }
}
