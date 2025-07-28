using PetConnect.BLL.Services.DTOs.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IChatService
    {
        public IEnumerable<UserMessageDto> LoadChat(string SenderId, string ReceiverId, int PageId = -1);

        public IEnumerable<UserBannerDto> LoadUserMessengersBySenderId(string SenderId);

        public bool IsOnline(string UserId);

        public UserDataDto GetUserData(string UserId);
    }
}
