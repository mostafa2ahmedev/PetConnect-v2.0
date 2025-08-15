using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        IEnumerable<Review> GetByCustomerId(string customerId);
        IEnumerable<Review> GetByDoctorId(string doctorId);
        bool AnyByAppointment(Guid appointmentId);
        bool DeleteReview(int reviewId);

    }

}
