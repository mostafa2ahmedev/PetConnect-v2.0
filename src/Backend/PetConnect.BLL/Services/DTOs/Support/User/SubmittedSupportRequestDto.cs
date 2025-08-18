using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Support.User
{
    public class SubmittedSupportRequestDto
    {

        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? CreatedAt { get; set; } = null!;
        public string? LastActivity { get; set; } = null!;
    }
}
