using PetConnect.BLL.Services.DTOs.Support;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface ISupportRequestService
    {

        bool CreateSupportRequest(string UserId,CreateSupportRequestDto supportRequestDto);

        bool UpdateSupportRequestStatus(UpdateSupportRequestDto updateSupportRequestDto);
        IEnumerable<SupportRequestDto> GetSupportRequests();
    }
}
