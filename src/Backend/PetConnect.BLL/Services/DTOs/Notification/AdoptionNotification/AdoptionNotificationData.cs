using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Notification.AdoptionNotification
{
    public class AdoptionNotificationData
    {

        public Guid Id { get; set; }
        public string Message { get; set; } = null!;

        public bool IsRead { get; set; }
        public DateTime Date { get; set; }


    }
}
