using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.BLL.Services.DTOs.Notification
{
    public class NotificationDTO
    {
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; }
    }
}
