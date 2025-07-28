using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Chat
{
    public class UserBannerDto
    {
        public string ReceiverName { get; set; } = null!;
        public string ImageURL { get; set; } = null!;
        public string LastMessage { get; set; } = null!;
        public DateTime LastMessageDate { get; set; } 
        public bool IsOnline { get; set; }
        public bool IsRead { get; set; }

    }
}
