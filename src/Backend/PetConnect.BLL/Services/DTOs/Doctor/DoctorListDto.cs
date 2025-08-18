using PetConnect.BLL.Services.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Doctor
{
    class DoctorListDto
    {
        public string Id { get; set; } = null!;
        public string? FullName { get; set; }
        public double AverageRating { get; set; } 
        public int ReviewsCount { get; set; }
        public List<ReviewSummaryDto> LastFiveReviews { get; set; } = new();
    }
}
