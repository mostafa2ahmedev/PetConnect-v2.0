using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.BLL.Services.DTOs.Notification;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface INotificationService
    {
        public List<NotificationDetailsDTO> GetAllNotificationsByUserId(string userId);
        public bool MarkNotificationAsRead(Guid notificatioId);

        public bool DeleteNotification(Guid notificatioId);

        public Task CreateAndSendNotification(string userId, NotificationDTO dto);

    }
}
