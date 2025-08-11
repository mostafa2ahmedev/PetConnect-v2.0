using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Support
{
    public class CreateSupportResponseDto
    {

        public string Message { get; set; } = null!;

        public int SupportRequestId { get; set; }

        public SupportRequestStatus Status { get; set; }
    }
}
