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
            if (PageId ==-1)
                Messages = _unitOfWork.UserMessagesRepository.GetAllQueryable()
                .Where(UM => (UM.SenderId == SenderId || UM.RecieverId == SenderId) && (UM.RecieverId == ReceiverId || UM.RecieverId == SenderId)).ToList();
            else
                Messages = _unitOfWork.UserMessagesRepository.GetAllQueryable()
               .Where(UM => (UM.SenderId == SenderId || UM.RecieverId == SenderId) && (UM.RecieverId == ReceiverId || UM.RecieverId == SenderId)).Skip((PageId * NumberOfMessages)).Take(NumberOfMessages).ToList();


            List<UserMessageDto> UserMessageListDtos = new List<UserMessageDto>();
            foreach (var UserMessage in Messages)
            {
                var UserMessageDto = new UserMessageDto() {
                    MessageId = UserMessage.MessageId,
                    SenderId = UserMessage.SenderId,
                    RecieverId = UserMessage.RecieverId,
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



        public IEnumerable<UserBannerDto> LoadUserMessengersBySenderId(string SenderId)
        {

            var messengers =
                 (from m in _unitOfWork.UserMessagesRepository.GetAllQueryable()
                 where m.SenderId == SenderId
                 select m)
                .AsEnumerable()
                .GroupBy(m => new { m.SenderId, m.RecieverId })
                .Select(group =>
                {
                    var lastMsg = group.LastOrDefault();
                    var user = GetUserData(lastMsg.RecieverId);
                    return new UserBannerDto
                    {
                        LastMessage = lastMsg?.Message,
                        LastMessageDate = lastMsg.SentDate,
                        ReceiverName = user.FullName,
                        IsOnline = IsOnline(lastMsg.RecieverId),
                        ImageURL = user.ImgUrl,
                        IsRead = lastMsg.IsRead 
                    };
                    })
                    .ToList();

            return messengers;
        }

        public UserDataDto GetUserData(string UserId)
        {
            var User = _unitOfWork.UserRepository.GetByID(UserId);

            return new UserDataDto() {
            FullName = User.FName + " "+ User.LName,
            ImgUrl = User.ImgUrl
            };


        }

        public bool IsOnline(string UserId)
        {
            var userConnection = _unitOfWork.UserConnectionRepository.GetByID(UserId);
            if (userConnection is { })
                return true;
            return false;
        }


    }
}
