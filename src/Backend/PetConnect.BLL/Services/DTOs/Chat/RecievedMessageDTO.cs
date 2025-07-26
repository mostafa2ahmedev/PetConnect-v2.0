using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Chat
{
    public class RecievedMessageDTO
    {
        public string SenderId { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
