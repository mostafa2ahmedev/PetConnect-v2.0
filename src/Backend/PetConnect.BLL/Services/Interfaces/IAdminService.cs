using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Admin;
using PetConnect.DAL.Data.Models;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IAdminService
    {
        public AdminDashboardDTO GetPendingDoctorsAndPets();
        public void ApproveDoctor(string id);
        public void ApprovePet(int id);



    }
}
