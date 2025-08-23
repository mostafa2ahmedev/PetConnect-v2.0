using PetConnect.BLL.Services.DTOs.Support.Admin;
using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public  interface IAdminSupportResponseService
    {
       Task<bool> CreateSupportResponse(CreateAdminSupportResponseDto SupportResponseDto);
       bool UpdatePriorityStatus(UpdatePriorityStatusDto UpdatePriorityStatusDto);

    }
}
