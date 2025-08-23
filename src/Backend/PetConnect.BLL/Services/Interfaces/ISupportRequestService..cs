using PetConnect.BLL.Services.DTOs.Support.Admin;
using PetConnect.BLL.Services.DTOs.Support.User;
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

        Task<bool> CreateSupportRequest(string UserId,CreateSupportRequestDto supportRequestDto);

        bool UpdateSupportRequestStatus(UpdateSupportRequestStatusDto updateSupportRequestDto);
        IEnumerable<AdminSupportRequestDto> GetAdminSupportRequests();



        IEnumerable<SubmittedSupportRequestDto> GetSubmittedSupportRequestsForUser(string UserId);


        SubmittedSupportRequestDetailsDto? GetSubmittedSupportRequestsDetails(string Role,string UserId,int supportRequestId);
    }
}
