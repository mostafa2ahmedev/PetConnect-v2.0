using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Identity;

namespace PetConnect.DAL.Data.Models
{
    public class UsersMessages
    {
        public int MessageId{ get; set; }
        public DateTime SentDate { get; set; }
        public string Message { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime? ReadDate { get; set; } = null;
        public UserMessageType MessageType{ get; set; }
        public string? AttachmentUrl { get; set; }
        public string SenderId { get; set; } = null!;
        public ApplicationUser Sender { get; set; } = null!;
        public string RecieverId { get; set; } = null!;
        public ApplicationUser Receiver { get; set; } = null!;

    }
}
