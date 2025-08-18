using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Review
{
    public class ReviewCreatedByCustToDocDTO
    {
        public string DoctorId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public string CustomerId { get; set; }
        public Guid AppointmentId { get; set; }
    }
}
