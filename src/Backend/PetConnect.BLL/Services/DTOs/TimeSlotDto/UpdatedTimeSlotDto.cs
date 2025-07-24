using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.TimeSlotDto
{
    class UpdatedTimeSlotDto
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "The Doctor Id cannot be null")]
        public string DoctorId { get; set; } = null!;
        [Required(ErrorMessage = "The Start time is required")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "The End time is required")]
        public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "Max Capacity is required")]
        public int MaxCapacity { get; set; }

        [Required(ErrorMessage = "Booked Count is required")]
        public int BookedCount { get; set; }

        public bool IsActive { get; set; }
    }
}
