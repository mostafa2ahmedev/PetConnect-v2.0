using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.AppointmentDto
{
    public class AppointmentDoctorProfileViewDTO
    {
        public Guid? Id { get; set; }
        public string? CustomerName { get; set; } 
        public string? DoctorName { get; set; }
        public DateTime SlotStartTime { get; set; }
        public DateTime SlotEndTime { get; set; }
        public string? PetName { get; set; }
        public string Status { get; set; } = "Available";
        public DateTime? CreatedAt { get; set; }
        public int MaxCapacity { get; set; }
        public int BookedCount { get; set; }
        public string? Notes { get; set; }
        public string? PetImg { get; set; }
        public string? CustomerImg { get; set; }
        public string? CustomerPhone { get; set; }
        public string DoctorId { get; set; }
        public bool IsReviewable { get; set; }
    }
}
