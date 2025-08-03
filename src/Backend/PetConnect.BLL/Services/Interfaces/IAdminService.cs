using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Admin;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.DAL.Data.Models;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IAdminService
    {
        public AdminDashboardDTO GetPendingDoctorsAndPets();
        public DoctorDetailsDTO? ApproveDoctor(string id);
        public Task<PetDetailsDto?> ApprovePet(int id);
        public Task<DoctorDetailsDTO?> RejectDoctor(string id, string message);
        public PetDetailsDto? RejectPet(int id, string message);
        public  Task<AdminStatisticsDTO> GetAdminStatistics();

        public CustomerDetailsDTO? GetProfile(string id);



    }
}
