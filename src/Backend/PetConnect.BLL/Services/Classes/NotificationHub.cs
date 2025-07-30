using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PetConnect.BLL.Services.DTOs.Notification;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.BLL.Services.Classes
{
    public class NotificationHub : Hub , INotificationHub
    {
        private readonly IUnitOfWork unitOfWork;

        public NotificationHub(IUnitOfWork _unitOfWork )
        {
            unitOfWork = _unitOfWork;
        }
        #region Send Notification Will Be Moved to Service
        //public  async Task SendNotication(string receiverId, NotificationDTO notificationDTO)
        //{
        //    //data base 
        //    var user = unitOfWork.UserRepository.GetByID(receiverId);
        //    if (user is not null)
        //    {
        //        var notification = new Notification()
        //        {
        //            CreatedAt = DateTime.UtcNow,
        //            IsRead = false , 
        //            NotificationType = notificationDTO.Type,
        //            UserId = receiverId,
        //            Message = notificationDTO.Message,
        //        };
        //        unitOfWork.NotificationRepository.Add(notification);
        //        unitOfWork.SaveChanges();
        //    }
        //    else { 
        //        return; 
        //    }
        //    //send to receiver 
        //    await Clients.User(receiverId).SendAsync("RecieveNotification", new
        //    {
        //        recieverId = receiverId,
        //        Message = notificationDTO.Message,
        //        MessageType = notificationDTO.Type,
        //        CreatedAt = DateTime.UtcNow,
        //        IsRead = false,
        //    });
        //} 
        #endregion
        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var connectionId = Context.ConnectionId;
            var userConnection = new UserConnection()
            {
                ConnectionId = connectionId,
                UserId = userId
            };
            unitOfWork.UserConnectionRepository.Add(userConnection);
            unitOfWork.SaveChanges();

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;

            UserConnection? res = unitOfWork.UserConnectionRepository.GetByID(connectionId);
            if (res is not null)
            {
                unitOfWork.UserConnectionRepository.Delete(res);
                unitOfWork.SaveChanges();
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
