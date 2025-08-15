 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.BLL.Services.DTOs.Account;
using PetConnect.DAL.Data.Identity;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IJwtService
    {
        Task<AuthResponse> CreateJwtToken(ApplicationUser user);
    }
}
