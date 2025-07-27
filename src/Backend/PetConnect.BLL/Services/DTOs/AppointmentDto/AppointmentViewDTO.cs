using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.AppointmentDto
{
    public class AppointmentViewDTO
    {
        public Guid Id { get; set; }

        public string DoctorId { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
        public Guid SlotId { get; set; }

        public int? PetId { get; set; }

        public AppointmentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
