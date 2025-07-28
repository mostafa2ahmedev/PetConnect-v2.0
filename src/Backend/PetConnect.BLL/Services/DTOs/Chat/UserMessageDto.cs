using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Chat
{
    public class UserMessageDto
    {
        public int MessageId { get; set; }
        public DateTime SentDate { get; set; }
        public string Message { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime? ReadDate { get; set; } = null;
        public UserMessageType MessageType { get; set; }
        public string? AttachmentUrl { get; set; }
        public string SenderId { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;


    }
}
