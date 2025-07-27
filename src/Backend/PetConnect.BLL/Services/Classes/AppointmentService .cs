using PetConnect.BLL.Services.DTOs.AppointmentDto;
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
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AppointmentViewDTO>> GetAllAppointmentsAsync()
        {
            var appointments = _unitOfWork.AppointmentsRepository.GetAll();

            return appointments.Select(a => new AppointmentViewDTO
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                CustomerId = a.CustomerId,
                SlotId = a.SlotId,
                PetId = a.PetId,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<AppointmentViewDTO?> GetAppointmentByIdAsync(Guid id)
        {
            var a = await _unitOfWork.AppointmentsRepository.GetByGuidAsync(id);
            if (a == null) return null;

            return new AppointmentViewDTO
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                CustomerId = a.CustomerId,
                SlotId = a.SlotId,
                PetId = a.PetId,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            };
        }

        public async Task<IEnumerable<AppointmentViewDTO>> GetAppointmentsByDoctorAsync(string doctorId)
        {
            var appointments = _unitOfWork.AppointmentsRepository
                .GetAll().Where(a => a.DoctorId == doctorId);

            return appointments.Select(a => new AppointmentViewDTO
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                CustomerId = a.CustomerId,
                SlotId = a.SlotId,
                PetId = a.PetId,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<IEnumerable<AppointmentViewDTO>> GetAppointmentsByCustomerAsync(string customerId)
        {
            var appointments = _unitOfWork.AppointmentsRepository
                .GetAll().Where(a => a.CustomerId == customerId);

            return appointments.Select(a => new AppointmentViewDTO
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                CustomerId = a.CustomerId,
                SlotId = a.SlotId,
                PetId = a.PetId,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<AppointmentViewDTO> AddAppointmentAsync(AppointmentCreateDTO dto)
        {
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                DoctorId = dto.DoctorId,
                CustomerId = dto.CustomerId,
                SlotId = dto.SlotId,
                PetId = dto.PetId,
                Status = AppointmentStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.AppointmentsRepository.Add(appointment);
            await _unitOfWork.SaveChangesAsync();

            return new AppointmentViewDTO
            {
                Id = appointment.Id,
                DoctorId = appointment.DoctorId,
                CustomerId = appointment.CustomerId,
                SlotId = appointment.SlotId,
                PetId = appointment.PetId,
                Status = appointment.Status,
                CreatedAt = appointment.CreatedAt
            };
        }

        public async Task<bool> CancelAppointmentAsync(Guid id)
        {
            var appointment = await _unitOfWork.AppointmentsRepository.GetByGuidAsync(id);
            if (appointment == null) return false;

            appointment.Status = AppointmentStatus.Cancelled;

            _unitOfWork.AppointmentsRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}

