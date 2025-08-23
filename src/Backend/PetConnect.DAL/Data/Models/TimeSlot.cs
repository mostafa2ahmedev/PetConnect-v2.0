using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class TimeSlot
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string DoctorId { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int MaxCapacity { get; set; }

        public int BookedCount { get; set; }

        public bool IsActive { get; set; }

        // Navigation properties
        public Doctor Doctor { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();

        public bool IsFull => BookedCount >= MaxCapacity;
    }
}
