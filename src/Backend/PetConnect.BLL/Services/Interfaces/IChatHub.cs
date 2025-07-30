using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IChatHub
    {
        public  Task SendMessage(string message, string recieverId, string? attachmentUrl);
    }
}
