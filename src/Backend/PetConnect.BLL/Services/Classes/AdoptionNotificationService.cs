using Microsoft.EntityFrameworkCore;
using PetConnect.BLL.Services.DTOs.Notification.AdoptionNotification;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class AdoptionNotificationService : IAdoptionNotificationService
    {
        private readonly IUnitOfWork unitOfWork;

        public AdoptionNotificationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void AddAdoptionNotification(string NotificationMessage ,string RecCustomerId)
        {
            var Notification = new Notification()
            {
                IsRead = false,
                Message = NotificationMessage,
                NotificationType = NotificationType.Adoption,
            };
            unitOfWork.NotificationRepository.Add(Notification);

            unitOfWork.SaveChanges();

            var AdoptionNotification = new AdoptionNotification() { 
            NotificationId = Notification.Id,
            CustomerId = RecCustomerId
            };

            unitOfWork.AdoptionNotificationRepository.Add(AdoptionNotification);

            unitOfWork.SaveChanges();
        }

        public IEnumerable<AdoptionNotificationData> GetAllAdoptionNotifications(string RecCustomerId)
        {
         return unitOfWork.NotificationRepository.GetAllQueryable().Where(N => N.NotificationType == NotificationType.Adoption).Include(N => N.AdoptionNotification)
                .Select(N => new AdoptionNotificationData()
                {
                    Id = N.Id,
                    Message = N.Message,
                    Date = N.CreatedAt,
                    IsRead = N.IsRead,
                }).ToList();
        }
    }
}
