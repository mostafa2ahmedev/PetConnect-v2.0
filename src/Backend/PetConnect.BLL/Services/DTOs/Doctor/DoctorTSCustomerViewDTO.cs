using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Doctor
{
    public class DoctorTSCustomerViewDTO
    {
        public Guid TimeSlotId { get; set; } 

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int MaxCapacity { get; set; }
        public int BookedCount { get; set; }
        public bool IsFull => BookedCount >= MaxCapacity;
        public int? PetId { get; set; }
        public string? PetName { get; set; }
        public string Status { get; set; } = "Available";
        //public ICollection<TimeSlot> TimeSlots { get; set; } = new HashSet<TimeSlot>();
        //public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
