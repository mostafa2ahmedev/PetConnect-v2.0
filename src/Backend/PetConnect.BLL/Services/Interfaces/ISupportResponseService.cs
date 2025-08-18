using PetConnect.BLL.Services.DTOs.Support;
using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public  interface ISupportResponseService
    {
        bool CreateSupportResponse(CreateSupportResponseDto SupportResponseDto);

    }
}
