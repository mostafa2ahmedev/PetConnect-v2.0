using PetConnect.BLL.Services.DTOs.Notification.AdoptionNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IAdoptionNotificationService
    {

        IEnumerable<AdoptionNotificationData>  GetAllAdoptionNotifications(string RecCustomerId);

        void AddAdoptionNotification(string NotificationMessage, string RecCustomerId);

    }
}
