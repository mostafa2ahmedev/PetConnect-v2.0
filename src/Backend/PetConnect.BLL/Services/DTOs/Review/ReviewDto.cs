using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Review
{
    class ReviewDto
    {
        public int Id { get; set; }
        public string? DoctorId { get; set; }
        public string? CustomerId { get; set; }
        public Guid AppointmentId { get; set; }
        public int Rating { get; set; }
        public string? ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public string? CustomerName { get; set; } 
    }
}
