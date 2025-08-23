using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Review
{
     public  class ReviewCreateDto
    {
        public Guid AppointmentId { get; set; } 
        public int Rating { get; set; } 
        public string? ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }


    }
}
