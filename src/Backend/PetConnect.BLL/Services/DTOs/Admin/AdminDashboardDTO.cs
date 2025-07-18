using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.BLL.Services.DTO.PetDto;

namespace PetConnect.BLL.Services.DTOs.Admin
{
    public class AdminDashboardDTO
    {
        public List<DoctorDetailsDTO> PendingDoctors { get; set; }
        public List<PetDetailsDto> PendingPets { get; set; }
    }
}
