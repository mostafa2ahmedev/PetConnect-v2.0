using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTOs.Support;
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
    public class SupportResponseService : ISupportResponseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISupportRequestService supportRequestService;
        private readonly IEmailService _emailService;

        public SupportResponseService(IUnitOfWork unitOfWork, ISupportRequestService supportRequestService, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.supportRequestService = supportRequestService;
            _emailService = emailService;
        }
        public bool CreateSupportResponse([FromBody]CreateSupportResponseDto supportResponseDto)
        {
            var SuppResponse = new SupportResponse()
            {
                Message = supportResponseDto.Message,
                SupportRequestId = supportResponseDto.SupportRequestId,

            };
            unitOfWork.SupportResponseRepository.Add(SuppResponse);

            supportRequestService.UpdateSupportRequestStatus(new UpdateSupportRequestDto()
            {
                SupportRequestId = supportResponseDto.SupportRequestId,
                SupportRequestStatus = supportResponseDto.Status,


            });
            var SuppRequestRecord = unitOfWork.SupportResponseRepository.GetUesrByRequestId(supportResponseDto.SupportRequestId);
            if (SuppRequestRecord is null)
                return false;

            var User = unitOfWork.UserRepository.GetByID(SuppRequestRecord.UserId);
            _emailService.SendEmailAsync(User!.Email!, supportResponseDto.Subject, supportResponseDto.Message);
            return unitOfWork.SaveChanges() >= 1;
        }
    }
}