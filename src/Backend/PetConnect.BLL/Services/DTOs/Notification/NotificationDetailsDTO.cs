using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.BLL.Services.DTOs.Notification
{
    public class NotificationDetailsDTO
    {
        public Guid NotificationId { get; set; }
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
