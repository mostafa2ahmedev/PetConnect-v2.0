using Microsoft.EntityFrameworkCore;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;


        public ReviewService(IReviewRepository reviewRepository , IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;

        }

        public IEnumerable<Review> GetByCustomerId(string customerId)
        {
            return _reviewRepository.GetByCustomerId(customerId);
        }

        public IEnumerable<Review> GetByDoctorId(string doctorId)
        {
            return _reviewRepository.GetByDoctorId(doctorId);
        }

        public bool AnyByAppointment(Guid appointmentId)
        {
            return _reviewRepository.AnyByAppointment(appointmentId);
        }

        public Review AddReview(Review review)
        {
            _reviewRepository.Add(review);
            _unitOfWork.SaveChanges();  
                return review;
        }

        public bool DeleteReview(Guid reviewId)
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
