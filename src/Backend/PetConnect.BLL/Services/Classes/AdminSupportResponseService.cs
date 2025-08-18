using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTOs.Blog;
using PetConnect.BLL.Services.DTOs.Support;
using PetConnect.BLL.Services.DTOs.Support.Admin;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class AdminSupportResponseService : IAdminSupportResponseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISupportRequestService supportRequestService;
        private readonly IEmailService _emailService;
        private readonly IAttachmentService _attachmentService;

        public AdminSupportResponseService(IUnitOfWork unitOfWork, ISupportRequestService supportRequestService, IEmailService emailService,IAttachmentService attachmentService)
        {
            this.unitOfWork = unitOfWork;
            this.supportRequestService = supportRequestService;
            _emailService = emailService;
            _attachmentService = attachmentService;
        }
        public async Task<bool> CreateSupportResponse([FromBody]CreateAdminSupportResponseDto supportResponseDto)
        {

            string? fileName = null;

            if (supportResponseDto.PictureUrl != null)
            {
                fileName = await _attachmentService.UploadAsync(supportResponseDto.PictureUrl, "img/support");
            }
            string? PictureUrl = fileName != null ? $"/assets/img/support/{fileName}" : null;

            var SuppResponse = new AdminSupportResponse()
            {
                Message = supportResponseDto.Message,
                SupportRequestId = supportResponseDto.SupportRequestId,
                LastActivity = DateTime.Now,
                PictureUrl  = PictureUrl



            };
            unitOfWork.SupportResponseRepository.Add(SuppResponse);

            supportRequestService.UpdateSupportRequestStatus(new UpdateSupportRequestStatusDto()
            {
                SupportRequestId = supportResponseDto.SupportRequestId,
                SupportRequestStatus = supportResponseDto.Status,
                LastActivity = DateTime.Now
               


            });
            var SuppRequestRecord = unitOfWork.SupportRequestRepository.GetUesrByRequestId(supportResponseDto.SupportRequestId);
            if (SuppRequestRecord is null)
                return false;

            
            var User = unitOfWork.UserRepository.GetByID(SuppRequestRecord.UserId);
            await _emailService.SendEmailAsync(User!.Email!, supportResponseDto.Subject, supportResponseDto.Message, PictureUrl);
            return unitOfWork.SaveChanges() >= 1;
        }

        public  bool UpdatePriorityStatus(UpdatePriorityStatusDto UpdatePriorityStatusDto)
        {
            var SuppRequestRecord = unitOfWork.SupportRequestRepository.GetUesrByRequestId(UpdatePriorityStatusDto.SupportRequestId);
            if (SuppRequestRecord is null)
                return false;

            SuppRequestRecord.Priority = UpdatePriorityStatusDto.Priority;
            SuppRequestRecord.LastActivity = DateTime.Now;

            unitOfWork.SupportRequestRepository.Update(SuppRequestRecord);
           

            return unitOfWork.SaveChanges()>0;


        }
    }
}