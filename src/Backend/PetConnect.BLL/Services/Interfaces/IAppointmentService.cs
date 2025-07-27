using PetConnect.BLL.Services.DTOs.AppointmentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
      public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentViewDTO>> GetAllAppointmentsAsync();
        Task<AppointmentViewDTO?> GetAppointmentByIdAsync(Guid id);
        Task<IEnumerable<AppointmentViewDTO>> GetAppointmentsByDoctorAsync(string doctorId);
        Task<IEnumerable<AppointmentViewDTO>> GetAppointmentsByCustomerAsync(string customerId);
        Task<AppointmentViewDTO> AddAppointmentAsync(AppointmentCreateDTO dto);
        Task<bool> CancelAppointmentAsync(Guid id);
    }
}
