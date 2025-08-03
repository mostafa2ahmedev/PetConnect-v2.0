
using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.BLL.Services.Classes
{
    public class ChatHub :Hub , IChatHub
    {
        private readonly IUnitOfWork unitOfWork;

        public ChatHub(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public async Task SendMessage(string message, string receiverId, string? attachmentUrl)
        {
            //save in DB 
            var userMessages = new UsersMessages()
            {
                AttachmentUrl = attachmentUrl,
                IsDeleted = false,
                IsRead = false,
                Message = message,
                MessageType = attachmentUrl == null ? UserMessageType.Text : UserMessageType.File,
                SenderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                RecieverId = receiverId,
            };
            unitOfWork.UserMessagesRepository.Add(userMessages);
            unitOfWork.SaveChanges();

            await Clients.User(receiverId).SendAsync("ReceiveMessage", new
            {
                SenderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                ReceiverId = receiverId,
                Message = message,
                SentDate = userMessages.SentDate,
                AttachmentUrl = attachmentUrl,
                MessageType = userMessages.MessageType.ToString()
            });
            await Clients.Caller.SendAsync("ReceiveMessage", new
            {
                SenderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                ReceiverId = receiverId,
                Message = message,
                SentDate = userMessages.SentDate,
                AttachmentUrl = attachmentUrl,
                MessageType = userMessages.MessageType.ToString()
            });

            await Clients.Caller.SendAsync("MessageSentConfirmation", new
            {
                ReceiverId = receiverId,
                Message = message,
                SentDate = userMessages.SentDate
            });


        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var connectionId = Context.ConnectionId;
            // this will notify each connected user expect the caller that this caller is now online
            await Clients.Others.SendAsync("UserOnline", userId);
            Console.WriteLine($"[Connected] ConnectionId: {connectionId}, UserId: {userId}");
            var userConnection = new UserConnection()
            {
                ConnectionId = connectionId,
                UserId = userId
            };
            unitOfWork.UserConnectionRepository.Add(userConnection);
            unitOfWork.SaveChanges();

             await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var connectionId = Context.ConnectionId;

            UserConnection? res = unitOfWork.UserConnectionRepository.GetByID(connectionId);
            if (res is not null)
            {
                await Clients.Others.SendAsync("UserOffline", userId);
                unitOfWork.UserConnectionRepository.Delete(res);
                unitOfWork.SaveChanges();
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
