using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTOs.Support.User;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class FollowUpSupportRequestService : IFollowUpSupportRequestService
    {
        private readonly IAttachmentService _attachmentService;
        private readonly IUnitOfWork _unitOfWork;

        public FollowUpSupportRequestService(IAttachmentService attachmentService,IUnitOfWork unitOfWork)
        {
            _attachmentService = attachmentService;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateFollowUpSupportRequest(string UserId, CreateFollowUpSupportRequestDto followUpSupportRequestDto)
        {
            var SupportRequest = _unitOfWork.SupportRequestRepository.GetByID(followUpSupportRequestDto.SupportRequestId);

            if (SupportRequest is null)
                return false;

            if (SupportRequest.Status == SupportRequestStatus.Closed)
                return false;   

                string? fileName = null;

            if (followUpSupportRequestDto.PictureUrl != null)
            {
                fileName = await _attachmentService.UploadAsync(followUpSupportRequestDto.PictureUrl, "img/support");
            }


            var FollowUpSuppRequest = new FollowUpSupportRequest()
            {
                Message = followUpSupportRequestDto.Message, 
                LastActivity = DateTime.Now,
                PictureUrl = fileName != null ? $"/assets/img/support/{fileName}" : null,
                SupportRequestId = followUpSupportRequestDto.SupportRequestId,          

            };

            _unitOfWork.FollowUpSupportRequestRepository.Add(FollowUpSuppRequest);

            SupportRequest.Status = SupportRequestStatus.Open;
            SupportRequest.LastActivity = DateTime.Now;
            _unitOfWork.SupportRequestRepository.Update(SupportRequest);

            return _unitOfWork.SaveChanges() >= 1;
        }
    }
}
