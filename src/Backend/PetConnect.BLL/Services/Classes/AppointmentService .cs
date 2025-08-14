using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetConnect.BLL.Services.DTOs.AppointmentDto;
using PetConnect.BLL.Services.DTOs.Notification;
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
        private readonly INotificationService _notificationService;

        public AppointmentService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
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
                Status = a.Status.ToString(),
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
                Status = a.Status.ToString(),
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
                Status = a.Status.ToString(),
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
                Status = a.Status.ToString(),
                CreatedAt = a.CreatedAt,

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
                Status = AppointmentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
            };
         
            _unitOfWork.AppointmentsRepository.Add(appointment);
            await _unitOfWork.SaveChangesAsync();
            await _notificationService.CreateAndSendNotification(dto.DoctorId, new NotificationDTO()
            {
                Message = $"You've got a new appointment!! ",
                Type = NotificationType.Approval
            });
            return new AppointmentViewDTO
            {
                Id = appointment.Id,
                DoctorId = appointment.DoctorId,
                CustomerId = appointment.CustomerId,
                SlotId = appointment.SlotId,
                PetId = appointment.PetId,
                Status = appointment.Status.ToString(),
                CreatedAt = appointment.CreatedAt,
                Notes = dto.Notes
            };
        }

        public IEnumerable<AppointmentDoctorProfileViewDTO> GetAppointmentsForDoctorProfile(string doctorId)
        {
            var appointment = _unitOfWork.AppointmentsRepository.GetAllQueryable()
                .Include(e => e.Customer).Include(e => e.Doctor).Include(e => e.Pet).Include(e => e.TimeSlot)
                .Where(e => e.DoctorId == doctorId);

            ICollection<AppointmentDoctorProfileViewDTO> appointments = new List<AppointmentDoctorProfileViewDTO>();
            foreach (var app in appointment)
            {
                appointments.Add(new AppointmentDoctorProfileViewDTO()
                {
                    Id = app.Id,
                    BookedCount = app.TimeSlot.BookedCount,
                    CreatedAt = app.CreatedAt,
                    CustomerName = $"{app.Customer.FName} {app.Customer.LName}",
                    MaxCapacity = app.TimeSlot.MaxCapacity,
                    Notes = "",
                    PetName = app.Pet.Name,
                    Status = app.Status.ToString(),
                    SlotStartTime = app.TimeSlot.StartTime,
                    SlotEndTime = app.TimeSlot.EndTime,
                    DoctorName = $"{app.Doctor.FName} {app.Doctor.LName}",
                    PetImg = app.Pet.ImgUrl,
                    CustomerImg = app.Customer.ImgUrl,
                    CustomerPhone= app.Customer.PhoneNumber
                });
            }
            return appointments;
        }

        public IEnumerable<AppointmentDoctorProfileViewDTO> GetAppointmentsForCustomerProfile(string customerId)
        {
            var appointment = _unitOfWork.AppointmentsRepository.GetAllQueryable()
                .Include(e => e.Customer).Include(e => e.Doctor).Include(e => e.Pet).Include(e => e.TimeSlot)
                .Where(e => e.CustomerId == customerId);

            ICollection<AppointmentDoctorProfileViewDTO> appointments = new List<AppointmentDoctorProfileViewDTO>();
            foreach (var app in appointment)
            {
                appointments.Add(new AppointmentDoctorProfileViewDTO()
                {
                    Id = app.Id,
                    CreatedAt = app.CreatedAt,
                    CustomerName = $"{app.Customer.FName} {app.Customer.LName}",
                    MaxCapacity = app.TimeSlot.MaxCapacity,
                    Notes = "",
                    PetName = app.Pet.Name,
                    Status = app.Status.ToString(),
                    SlotStartTime = app.TimeSlot.StartTime,
                    SlotEndTime = app.TimeSlot.EndTime,
                    DoctorName = $"{app.Doctor.FName} {app.Doctor.LName}",
                    PetImg = app.Pet.ImgUrl,
                    CustomerImg = app.Customer.ImgUrl,
                    CustomerPhone = app.Customer.PhoneNumber
                });
            }
            return appointments;
        }


        public async Task<bool> CancelAppointmentAsync(Guid id)
        {
            var appointment = await _unitOfWork.AppointmentsRepository.GetByGuidAsync(id);
            if (appointment == null || appointment.Status == AppointmentStatus.Cancelled) return false;

            appointment.Status = AppointmentStatus.Cancelled;
            await _notificationService.CreateAndSendNotification(appointment.CustomerId, new NotificationDTO()
            {
                Message = $"Your appointment was cancelled ): ",
                Type = NotificationType.Approval
            });
            _unitOfWork.AppointmentsRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ConfirmAppointmentAsync(Guid id)
        {
            //var appointment = await _unitOfWork.AppointmentsRepository.GetByGuidAsync(id);
            var appointment = _unitOfWork.AppointmentsRepository.GetAllQueryable().
                Include(e => e.TimeSlot).SingleOrDefault(e => e.Id == id);

            if (appointment == null || appointment.TimeSlot.IsFull || appointment.Status != AppointmentStatus.Pending)
                return false;
            await _notificationService.CreateAndSendNotification(appointment.CustomerId, new NotificationDTO()
            {
                Message = $"Your appointment was confirmed ",
                Type = NotificationType.Approval
            }); appointment.TimeSlot.BookedCount += 1;


            appointment.Status = AppointmentStatus.Confirmed;

            _unitOfWork.AppointmentsRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CompleteAppointmentAsync(Guid id)
        {
            var appointment = await _unitOfWork.AppointmentsRepository.GetByGuidAsync(id);
            if (appointment == null || appointment.Status == AppointmentStatus.Completed) return false;

            appointment.Status = AppointmentStatus.Completed;
            await _notificationService.CreateAndSendNotification(appointment.CustomerId, new NotificationDTO()
            {
                Message = $"Your appointment is complete. Let us know your feedback! ",
                Type = NotificationType.Approval
            });
            _unitOfWork.AppointmentsRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> BookAppointmentAsync(Guid id)
        {
            var appointmentByCustomer = await _unitOfWork.AppointmentsRepository.GetByGuidAsync(id);
            if (appointmentByCustomer == null || appointmentByCustomer.TimeSlot?.StartTime == null || appointmentByCustomer.Status != AppointmentStatus.Available)
                return false;

            var targetDate = appointmentByCustomer.TimeSlot.StartTime.Date;
            var customerId = appointmentByCustomer.CustomerId;

            // Get all appointments for the same customer on the same day checks for conflicts
            var conflictExists = await _unitOfWork.AppointmentsRepository.GetAllQueryable()
                .Include(e => e.TimeSlot)
                .AnyAsync(e =>
                    e.CustomerId == customerId &&
                    e.TimeSlot != null &&
                    e.TimeSlot.StartTime.Date == targetDate);

            if (conflictExists)
                return false;
            await _notificationService.CreateAndSendNotification(appointmentByCustomer.DoctorId, new NotificationDTO()
            {
                Message = $"You've got a new appointment!!'",
                Type = NotificationType.Approval
            });
            appointmentByCustomer.Status = AppointmentStatus.Pending;
            _unitOfWork.AppointmentsRepository.Update(appointmentByCustomer);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public IEnumerable<AppointmentsAvailableForCurrDocCustomerDTO> GetCurrentTimeSlotsAvailableDocCustomer(string doctorId, string customerId)
        {
            //var appointment = _unitOfWork.AppointmentsRepository.GetAllQueryable()
            //    .Include(e => e.Customer).Include(e => e.Doctor).Include(e => e.Pet).Include(e => e.TimeSlot)
            //    .Where(e => e.DoctorId == doctorId && e.CustomerId==customerId);

            //ICollection<AppointmentsAvailableForCurrDocCustomerDTO> appointments = new List<AppointmentsAvailableForCurrDocCustomerDTO>();
            //foreach (var app in appointment)
            //{
            //    appointments.Add(new AppointmentsAvailableForCurrDocCustomerDTO()
            //    {
            //        Id = app.Id ,
            //        BookedCount = app.TimeSlot.BookedCount,
            //        CreatedAt = app.CreatedAt ,
            //        CustomerName = $"{app.Customer.FName} {app.Customer.LName}" ?? "",
            //        MaxCapacity = app.TimeSlot.MaxCapacity,
            //        Notes = "",
            //        PetName = app.Pet.Name ?? "",
            //        Status = app.Status.ToString() ?? "Available",
            //        SlotStartTime = app.TimeSlot.StartTime,
            //        SlotEndTime = app.TimeSlot.EndTime,
            //        CustomerId= app.CustomerId?? "None",
            //        SlotId= app.TimeSlot.Id
            //    });
            //}
            //return appointments;

            var timeSlots = _unitOfWork.TimeSlotsRepository.GetAllQueryable()
                .Where(t => t.DoctorId == doctorId)
                .Include(t => t.Appointments);

            var result = timeSlots.Select(slot => new AppointmentsAvailableForCurrDocCustomerDTO
            {
                SlotId = slot.Id,
                SlotStartTime = slot.StartTime,
                SlotEndTime = slot.EndTime,
                BookedCount = slot.BookedCount,
                MaxCapacity = slot.MaxCapacity,
                Status = slot.Appointments.Any(a => a.CustomerId == customerId)
                            ? slot.Appointments.First(a => a.CustomerId == customerId).Status.ToString()
                            : "Available",
                CustomerName = slot.Appointments.Any(a => a.CustomerId == customerId)
                            ? $"{slot.Appointments.First(a => a.CustomerId == customerId).Customer.FName} {slot.Appointments.First(a => a.CustomerId == customerId).Customer.LName}"
                            : "",
                //PetName = slot.Appointments.FirstOrDefault(a => a.CustomerId == customerId)?.Pet?.Name ?? "",
                CustomerId = customerId,
                Notes = ""
            }).ToList();
            return result;
        } 
    }
    
}

