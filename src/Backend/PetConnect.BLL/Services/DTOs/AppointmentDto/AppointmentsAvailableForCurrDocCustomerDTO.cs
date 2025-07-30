using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.AppointmentDto
{
    public class AppointmentsAvailableForCurrDocCustomerDTO
    {
        public Guid? Id { get; set; }
        public Guid SlotId { get; set; } 
        public string? CustomerName { get; set; }
        public string? CustomerId { get; set; }
        public DateTime SlotStartTime { get; set; }
        public DateTime SlotEndTime { get; set; }
        public string? PetName { get; set; }
        public string Status { get; set; } = "Available";
        public DateTime? CreatedAt { get; set; }
        public int MaxCapacity { get; set; }
        public int BookedCount { get; set; }
        public string? Notes { get; set; }
    }
}
