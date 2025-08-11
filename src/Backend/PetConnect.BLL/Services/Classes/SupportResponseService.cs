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
    

        public SupportResponseService(IUnitOfWork unitOfWork,ISupportRequestService supportRequestService)
        {
            this.unitOfWork = unitOfWork;
            this.supportRequestService = supportRequestService;

        }
        public bool CreateSupportResponse(CreateSupportResponseDto supportResponseDto)
        {
            var SuppResponse = new SupportResponse() {
            Message = supportResponseDto.Message,
            SupportRequestId = supportResponseDto.SupportRequestId,
            
            };
            unitOfWork.SupportResponseRepository.Add(SuppResponse);

            supportRequestService.UpdateSupportRequestStatus(new UpdateSupportRequestDto() {
            SupportRequestId = supportResponseDto.SupportRequestId,
            SupportRequestStatus = supportResponseDto.Status,
            

            });


           return unitOfWork.SaveChanges()>=1;
        }
    }
}
