using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.AppointmentDto
{
     public class AppointmentCreateDTO
    {
        [Required(ErrorMessage = "Doctor Id is required")]
        public string DoctorId { get; set; } = null!;

        [Required(ErrorMessage = "Customer Id is required")]
        public string CustomerId { get; set; } = null!;

        [Required(ErrorMessage = "TimeSlot Id is required")]
        public Guid SlotId { get; set; }

        public int? PetId { get; set; }
    }
}
