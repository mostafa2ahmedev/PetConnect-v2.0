using Microsoft.AspNetCore.Mvc;
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
        Task<bool> ConfirmAppointmentAsync(Guid id);
        Task<bool> CompleteAppointmentAsync(Guid id);
        Task<bool> BookAppointmentAsync(Guid id);

        IEnumerable<AppointmentDoctorProfileViewDTO> GetAppointmentsForDoctorProfile(string doctorId);
        IEnumerable<AppointmentsAvailableForCurrDocCustomerDTO> GetCurrentTimeSlotsAvailableDocCustomer(string doctorId, string customerId);
    }
}
