using PetConnect.BLL.Services.DTOs.Support;
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

        public SupportRequestService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public bool CreateSupportRequest(string UserId,CreateSupportRequestDto supportRequestDto)
        {
            var SuppRequest = new SupportRequest() { 
            Message = supportRequestDto.Message,    
            Status = SupportRequestStatus.Open, 
            UserId = UserId,
            Type = supportRequestDto.Type,
            
            };

            unitOfWork.SupportRequestRepository.Add(SuppRequest);
            return unitOfWork.SaveChanges()>=1;
           
        }

        public IEnumerable<SupportRequestDto> GetSupportRequests()
        {
          return unitOfWork.SupportRequestRepository.GetSupportRequestsWithUserData().Select(SR=>new SupportRequestDto() { 
          Id = SR.Id,
          Message = SR.Message,
          Status= SR.Status,
          Type = SR.Type,
          UserId= SR.UserId,
          UserName = SR.User.UserName!,
          UserEmail =SR.User.Email!
          });
        }

        public bool UpdateSupportRequestStatus(UpdateSupportRequestDto updateSupportRequestDto)
        {
            var SupportRequest = unitOfWork.SupportRequestRepository.GetByID(updateSupportRequestDto.SupportRequestId);

            if(SupportRequest is null)
                return false;
            
            SupportRequest.Status = updateSupportRequestDto.SupportRequestStatus;
            unitOfWork.SupportRequestRepository.Update(SupportRequest);
            unitOfWork.SaveChanges();
            return true;
        }
    }
}
