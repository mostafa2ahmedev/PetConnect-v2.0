using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Support
{
    public class SupportRequestDto
    {
        public int Id { get; set; }
        public SupportRequestType Type { get; set; }

        public SupportRequestStatus Status { get; set; }

        public string Message { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;

    }
}
