using PetConnect.BLL.Services.DTOs.Review;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IReviewService
    {
        IEnumerable<ReviewDto> GetByDoctorId(string doctorId);
        IEnumerable<ReviewDto> GetByCustomerId(string customerId);
        bool AnyByAppointment(Guid appointmentId);
        ReviewDto AddReview(ReviewCreateDto dto);

        bool DeleteReview(int reviewId);
        IEnumerable<ReviewByIdDoctorDTO> GetDoctor(string doctorId);
        IEnumerable<ReviewByCustomerDTO> GetCustomer(string customerId);
        bool AddCustomerReview(ReviewCreatedByCustToDocDTO reviewDTO);

    }
}