using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Chat
{
    public class UserDataDto
    {
        public string FullName { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;

        public string UserId { get; set; }

    }
}
