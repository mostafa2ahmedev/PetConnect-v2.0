using PetConnect.BLL.Services.DTOs.Chat;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork  = unitOfWork;
        }

 
        public IEnumerable<UserMessageDto> LoadChat(string SenderId, string ReceiverId,int PageId = -1)
        {
            var NumberOfMessages = 10;
            List<UsersMessages> Messages = new List<UsersMessages>();
            if (PageId == -1)
                Messages = _unitOfWork.UserMessagesRepository.GetAllQueryable()
                //.Where(UM => (UM.SenderId == SenderId || UM.RecieverId == SenderId) && (UM.RecieverId == ReceiverId || UM.RecieverId == SenderId)).ToList();
            .Where(UM => (UM.SenderId == SenderId && UM.RecieverId == ReceiverId) || (UM.RecieverId == SenderId && UM.SenderId == ReceiverId)).ToList();
            else
                Messages = _unitOfWork.UserMessagesRepository.GetAllQueryable()
               .Where(UM => (UM.SenderId == SenderId || UM.RecieverId == SenderId) && (UM.RecieverId == ReceiverId || UM.RecieverId == SenderId)).Skip((PageId * NumberOfMessages)).Take(NumberOfMessages).ToList();


            List<UserMessageDto> UserMessageListDtos = new List<UserMessageDto>();
            foreach (var UserMessage in Messages)
            {
                var UserMessageDto = new UserMessageDto() {
                    MessageId = UserMessage.MessageId,
                    SenderId = UserMessage.SenderId,
                    ReceiverId = UserMessage.RecieverId,
                    Message = UserMessage.Message,
                    IsDeleted = UserMessage.IsDeleted,
                    IsRead = UserMessage.IsRead,
                    ReadDate = UserMessage.ReadDate,
                    SentDate = UserMessage.SentDate,
                    AttachmentUrl = UserMessage.AttachmentUrl,
                    MessageType = UserMessage.MessageType
                };
                UserMessageListDtos.Add(UserMessageDto);

            }
            return UserMessageListDtos;
        }



        public IEnumerable<UserBannerDto> LoadUserMessengersBySenderId(string userId)
        {
            var messages = _unitOfWork.UserMessagesRepository.GetAllQueryable()
                .Where(m => m.SenderId == userId || m.RecieverId == userId) // sent OR received
                .AsEnumerable()
                .GroupBy(m => m.SenderId == userId ? m.RecieverId : m.SenderId) // group by other user's id
                .Select(group =>
                {
                    var lastMsg = group.OrderBy(m => m.SentDate).LastOrDefault();
                    var otherUserId = group.Key; // the ID of the other person in the conversation
                    var user = GetUserData(otherUserId);

                    return new UserBannerDto
                    {
                        LastMessage = lastMsg?.Message,
                        LastMessageDate = lastMsg.SentDate,
                        ReceiverName = user.FullName,
                        IsOnline = IsOnline(otherUserId),
                        ImageURL = user.ImgUrl,
                        IsRead = lastMsg?.IsRead ?? false,
                        UserId = user.UserId
                    };
                })
                .OrderByDescending(u => u.LastMessageDate) // newest conversations first
                .ToList();

            return messages;
        }

        public UserDataDto GetUserData(string UserId)
        {
            var User = _unitOfWork.UserRepository.GetByID(UserId);

            return new UserDataDto() {
            FullName = User.FName + " "+ User.LName,
            ImgUrl = User.ImgUrl,
            UserId = User.Id

            };


        }

        public bool IsOnline(string UserId)
        {
            var userConnection = _unitOfWork.UserConnectionRepository.GetAllQueryable().FirstOrDefault(C => C.UserId == UserId);
            if (userConnection is not null )
                return true;
            return false;
        }

        //public string UploadImage()


    }
}
