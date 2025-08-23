using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PetConnect.BLL.Services.DTOs.Notification;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.BLL.Services.Classes
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public NotificationService(IUnitOfWork _unitOfWork , IHubContext<NotificationHub> notificationHub)
        {
            unitOfWork = _unitOfWork;
            _notificationHub = notificationHub;
        }

        public  List<NotificationDetailsDTO> GetAllNotificationsByUserId(string userId) 
        {
            return unitOfWork.NotificationRepository.GetAllQueryable().Where(N => N.UserId == userId).
                Select(N => new NotificationDetailsDTO() 
                    {
                        NotificationId= N.Id,
                        CreatedAt = N.CreatedAt,
                        IsRead = N.IsRead , 
                        Message = N.Message,
                        Type = N.NotificationType
                    }).ToList() ;
        }

        public bool MarkNotificationAsRead(Guid notificatioId) 
        {
            var notification = unitOfWork.NotificationRepository.GetById(notificatioId);
            if (notification is not null) 
            {
                notification.IsRead = true;
                unitOfWork.NotificationRepository.Update(notification);
                unitOfWork.SaveChanges();
                return true; 
            }
            return false; 
        }

        public bool DeleteNotification(Guid notificatioId) 
        {
            var notification = unitOfWork.NotificationRepository.GetById(notificatioId);
            if (notification is not null)
            {
                unitOfWork.NotificationRepository.Delete(notification);
                unitOfWork.SaveChanges();
                return true;
            }
            return false;
        }

        public  async Task CreateAndSendNotification(string userId, NotificationDTO dto)
        {
            var notification = new Notification
            {
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Message = dto.Message,
                NotificationType = dto.Type,
                UserId = userId
            };

            unitOfWork.NotificationRepository.Add(notification);
            unitOfWork.SaveChanges();

            await _notificationHub.Clients.User(userId).SendAsync("ReceiveNotification", new
            {
                receiverId = userId,
                message = dto.Message,
                type = dto.Type,
                createdAt = notification.CreatedAt,
                isRead = false
            });
        }

   
    }
}
