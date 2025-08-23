using PetConnect.BLL.Services.DTOs.Support.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IFollowUpSupportRequestService
    {
        Task<bool> CreateFollowUpSupportRequest(string UserId, CreateFollowUpSupportRequestDto followUpSupportRequestDto);
    }
}
