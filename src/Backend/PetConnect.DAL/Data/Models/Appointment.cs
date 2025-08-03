using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.DAL.Data.Models
{
    public class Appointment
    {
        public Guid Id { get; set; } =  Guid.NewGuid();

        public Guid SlotId { get; set; }

        public string DoctorId { get; set; } = null!;

        public string CustomerId { get; set; } = null!;

        public int? PetId { get; set; }  

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Available;  // Pending and waiting for doctor to confirm

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public TimeSlot TimeSlot { get; set; } = null!; 

        public Doctor Doctor { get; set; } = null!;

        public Customer Customer { get; set; } = null!;

        public Pet Pet { get; set; } = null!;
    }

}

