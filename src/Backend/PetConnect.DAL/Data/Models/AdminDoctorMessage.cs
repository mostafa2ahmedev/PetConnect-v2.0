using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.DAL.Data.Models
{
    public class AdminDoctorMessage
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public AdminMessageType MessageType { get; set; }
        public string Message { get; set; } = null!;
        public string DoctorId { get; set; } = null!;
        public Doctor Doctor{ get; set; } = null!;
    }
}
