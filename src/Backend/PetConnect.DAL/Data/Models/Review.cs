using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public string? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public Guid AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }


        public int Rating { get; set; }
        public string? ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        
    }
}
