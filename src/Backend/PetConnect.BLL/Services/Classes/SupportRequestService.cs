using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTOs.Blog;
using PetConnect.BLL.Services.DTOs.Support;
using PetConnect.BLL.Services.DTOs.Support.Admin;
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
    public class SupportRequestService : ISupportRequestService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public SupportRequestService(IUnitOfWork unitOfWork,IAttachmentService attachmentService)
        {
            this.unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }
        public async Task<bool> CreateSupportRequest(string UserId, CreateSupportRequestDto supportRequestDto)
        {
            string? fileName = null;

            if (supportRequestDto.PictureUrl != null)
            {
                fileName = await _attachmentService.UploadAsync(supportRequestDto.PictureUrl, "img/support");
            }
            var SuppRequest = new SupportRequest()
            {
                Message = supportRequestDto.Message,
                Status = SupportRequestStatus.Open,
                UserId = UserId,
                Type = supportRequestDto.Type,
                LastActivity = DateTime.Now,
                PictureUrl = fileName != null ? $"/assets/img/support/{fileName}" : null,
                Priority = SupportRequestPriority.Low,
                
             
            };

            unitOfWork.SupportRequestRepository.Add(SuppRequest);
            return unitOfWork.SaveChanges() >= 1;

        }

        public IEnumerable<AdminSupportRequestDto> GetAdminSupportRequests()
        {
            return unitOfWork.SupportRequestRepository.GetSupportRequestsWithUserDataForAdmin().Select(SR => new AdminSupportRequestDto()
            {
                Id = SR.Id,
                Message = SR.Message,
                SupportRequestStatus = SR.Status.ToString(),
                SupportRequestType = SR.Type.ToString(),
                UserId = SR.UserId,
                UserName = SR.User.FName + " " + SR.User.LName!,
                UserEmail = SR.User.Email!,
                CreatedAt = SR.CreatedAt.ToString(),
                LastActivity = SR.LastActivity.ToString(),
                Priority  =SR.Priority.ToString()
                
            });
        }

      

        public bool UpdateSupportRequestStatus(UpdateSupportRequestStatusDto updateSupportRequestDto)
        {
            var SupportRequest = unitOfWork.SupportRequestRepository.GetByID(updateSupportRequestDto.SupportRequestId);

            if (SupportRequest is null)
                return false;

            SupportRequest.Status = updateSupportRequestDto.SupportRequestStatus;
            SupportRequest.LastActivity = DateTime.Now;
            unitOfWork.SupportRequestRepository.Update(SupportRequest);

            return true;
        }



        public IEnumerable<SubmittedSupportRequestDto> GetSubmittedSupportRequestsForUser(string UserId)
        {
           var submittedList = unitOfWork.SupportRequestRepository.GetSupportRequestsForUser(UserId).Select(SSR=> new SubmittedSupportRequestDto() { 
           Id = SSR.Id,
           CreatedAt = SSR.CreatedAt?.ToString(),
           LastActivity =SSR.LastActivity.ToString(),
           Message = SSR.Message,
           Priority = SSR.Priority.ToString(),
           Status = SSR.Status.ToString(),
           Type = SSR.Type.ToString(),
           });
            return submittedList;


        }

        public SubmittedSupportRequestDetailsDto? GetSubmittedSupportRequestsDetails(string Role, string UserId, int supportRequestId)
        {
            SupportRequest? SupportRequest =unitOfWork.SupportRequestRepository.GetSupportRequestsDetailsForUser(supportRequestId);

            if (SupportRequest is null)
                return null;


            if (Role == "Customer" || Role =="Seller" ||Role =="Doctor") 
                if (UserId != SupportRequest.UserId)                 
                    return null;
                
                
        
            var Messages = new List<SupportMessageDto>();

            foreach (var item in SupportRequest.AdminSupportResponses)
            {
                Messages.Add(new SupportMessageDto() {
                CreatedAt = item.CreatedAt.ToString(),
                Message = item.Message,
                PictureUrl = item.PictureUrl,
                Sender = "Admin"
                });

            }
            foreach (var item in SupportRequest.FollowUpSupportRequests)
            {
                Messages.Add(new SupportMessageDto()
                {
                    CreatedAt = item.CreatedAt.ToString(),
                    Message = item.Message,
                    PictureUrl = item.PictureUrl,
                    Sender = SupportRequest.User.FName +" "+ SupportRequest.User.LName
                });

            }
           var OrderedMessages= Messages.OrderBy(SM => SM.CreatedAt).ToList();

            return new SubmittedSupportRequestDetailsDto() { 
            Id = SupportRequest.Id,
            CreatedAt = SupportRequest.CreatedAt.ToString(),    
            LastActivity = SupportRequest.LastActivity.ToString(),
            Message = SupportRequest.Message,
            Priority = SupportRequest.Priority.ToString(),
            Status = SupportRequest.Status.ToString(),  
            Type = SupportRequest.Type.ToString(),
            SupportMessages = OrderedMessages

            }; 
        }

 


    }
}