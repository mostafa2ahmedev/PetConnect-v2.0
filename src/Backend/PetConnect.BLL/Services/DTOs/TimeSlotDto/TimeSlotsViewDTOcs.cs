using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.TimeSlotDto
{
    public class TimeSlotsViewDTOcs
    {
        public Guid Id { get; set; } 

        public string DoctorId { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public string? Status { get; set; } = AppointmentStatus.Available.ToString();
        public int MaxCapacity { get; set; }

        public int BookedCount { get; set; }

        public bool IsActive { get; set; }
        public bool IsFull => BookedCount >= MaxCapacity;


        //public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();





    }
}
