using Microsoft.EntityFrameworkCore;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Review> GetByCustomerId(string customerId)
        {
            return _context.Review 
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.ReviewDate)
                .Include(r => r.Doctor)
                .Include(r => r.Customer)
                .ToList();
        }

        public IEnumerable<Review> GetByDoctorId(string doctorId)
        {
            return _context.Review 
                .Where(r => r.DoctorId == doctorId)
                .OrderByDescending(r => r.ReviewDate)
                .Include(r => r.Customer)
                .ToList();
        }

        public bool AnyByAppointment(Guid appointmentId)
        {
            return _context.Review 
                .Any(r => r.AppointmentId == appointmentId);
        }
        public bool DeleteReview(int reviewId)
        {
            var review = _context.Review.Find(reviewId);
            if (review == null) return false;

            _context.Review.Remove(review);
            _context.SaveChanges();
            return true;
        }

    }
}
