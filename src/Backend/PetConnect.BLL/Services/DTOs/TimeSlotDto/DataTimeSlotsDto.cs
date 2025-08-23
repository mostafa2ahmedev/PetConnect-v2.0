using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.TimeSlotDto
{
    public class DataTimeSlotsDto
    {

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Status { get; set; } = "Available";
        public int MaxCapacity { get; set; }

        public int BookedCount { get; set; }

        public bool IsActive { get; set; }
        public Guid Id { get; set; }
    }
}
