using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Support.Admin
{
    public class UpdateSupportRequestStatusDto
    {
        public SupportRequestStatus SupportRequestStatus { get; set; }

        public int SupportRequestId { get; set; }

        public DateTime LastActivity { get; set; }

    }
}
