using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Support.Admin
{
    public class AdminSupportRequestDto
    {
        public int Id { get; set; }
        public string SupportRequestType { get; set; } = null!;

        public string SupportRequestStatus { get; set; } = null!;

        public string Priority { get; set; } = null!;

        public string? CreatedAt { get; set; }

        public string? LastActivity { get; set; }
        public string Message { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;

    }
}
