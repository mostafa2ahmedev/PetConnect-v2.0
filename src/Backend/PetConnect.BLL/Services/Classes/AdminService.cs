using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Admin;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.BLL.Services.DTOs.Notification;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.BLL.Services.Classes
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationService notificationService;

        public AdminService(IUnitOfWork _unitOfWork, INotificationService _notificationService)
        {
            unitOfWork = _unitOfWork;
            notificationService = _notificationService;
        }

        public AdminDashboardDTO GetPendingDoctorsAndPets()
        {
            var pendingDoctors = unitOfWork.DoctorRepository.GetAll()
                .Where(d => !d.IsApproved && !d.IsDeleted)
                .Select(d => new DoctorDetailsDTO
                {
                    Id = d.Id,
                    FName = d.FName,
                    LName = d.LName,
                    ImgUrl = d.ImgUrl ?? "/assets/img/default-doctor.jpg",
                    PetSpecialty = d.PetSpecialty.ToString(),
                    Gender = d.Gender.ToString(),
                    PricePerHour = d.PricePerHour,
                    CertificateUrl = d.CertificateUrl,
                    Street = d.Address.Street,
                    City = d.Address.City,
                    PhoneNumber = d.PhoneNumber,
                    IsApproved = d.IsApproved,
                    IsDeleted = d.IsDeleted

                }).ToList();

            var pendingPets = unitOfWork.PetRepository.GetPendingPetsWithBreedAndCategory()
                .Where(p => !p.IsApproved && !p.IsDeleted)
                .Select(p => new PetDetailsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Status = p.Status,
                    IsApproved = p.IsApproved,
                    Ownership = p.Ownership,
                    ImgUrl = "https://localhost:7102/assets/petimages/" + p.ImgUrl,
                    BreadName = p.Breed.Name,
                    CategoryName = p.Breed.Category.Name,
                    IsDeleted = p.IsDeleted
                }).ToList();

            return new AdminDashboardDTO
            {
                PendingDoctors = pendingDoctors,
                PendingPets = pendingPets
            };
        }

        public DoctorDetailsDTO? ApproveDoctor(string id)
        {
            Doctor? doctor = unitOfWork.DoctorRepository.GetByID(id);
            if (doctor is not null)
            {
                doctor.IsApproved = true;
                doctor.IsDeleted = false;
                unitOfWork.DoctorRepository.Update(doctor);
                unitOfWork.SaveChanges();

                DoctorDetailsDTO dto = new DoctorDetailsDTO()
                {
                    Id = doctor.Id,
                    IsApproved = doctor.IsApproved,
                    PetSpecialty = doctor.PetSpecialty.ToString(),
                    FName = doctor.FName,
                    LName = doctor.LName,
                    City = doctor.Address.City,
                    Street = doctor.Address.Street,
                    PricePerHour = doctor.PricePerHour
                };
                notificationService.CreateAndSendNotification(id, new NotificationDTO
                {
                    Message = "You Account Has Been Approved.",
                    Type = NotificationType.Approval,
                });
                return dto;
            }
            return null;
        }

        public async Task<PetDetailsDto?> ApprovePet(int id)
        {
            Pet? pet = unitOfWork.PetRepository.GetByID(id);
            var userId = unitOfWork.CustomerAddedPetsRepository.GetAllQueryable().FirstOrDefault(C => C.PetId == id)?.CustomerId;
            if (pet is not null)
            {
                pet.IsApproved = true;
                pet.IsDeleted = false;
                unitOfWork.PetRepository.Update(pet);
                unitOfWork.SaveChanges();

                PetDetailsDto dto = new PetDetailsDto()
                {
                    Name = pet.Name,
                    Id = pet.Id,
                    Status = pet.Status,
                    IsApproved = pet.IsApproved
                };
               await notificationService.CreateAndSendNotification(userId, new NotificationDTO()
                {
                    Message = $"Your Pet {pet.Name} With Id {pet.Id} Has Been Approved.",
                    Type = NotificationType.Approval
                });
                return dto;
            }
            return null;
        }

        public async Task<DoctorDetailsDTO?> RejectDoctor(string id, string message)
        {
            var doctor = unitOfWork.DoctorRepository.GetByID(id);
            if (doctor is not null)
            {
                //update doctor
                doctor.IsDeleted = true;
                doctor.IsApproved = false;
                unitOfWork.DoctorRepository.Update(doctor);
                //add the message to the database
                AdminDoctorMessage adminDoctorMessage = new AdminDoctorMessage()
                {
                    MessageType = AdminMessageType.Rejection,
                    Message = message,
                    DoctorId = id
                };
                unitOfWork.AdminDoctorMessageRepository.Add(adminDoctorMessage);
                unitOfWork.SaveChanges();
                //return the details to show in the API result 
                DoctorDetailsDTO dto = new DoctorDetailsDTO()
                {
                    Id = doctor.Id,
                    IsApproved = doctor.IsApproved,
                    PetSpecialty = doctor.PetSpecialty.ToString(),
                    FName = doctor.FName,
                    LName = doctor.LName,
                    City = doctor.Address.City,
                    Street = doctor.Address.Street,
                    PricePerHour = doctor.PricePerHour,
                    IsDeleted = doctor.IsDeleted
                };
               await  notificationService.CreateAndSendNotification(id, new NotificationDTO
                {
                    Message = "You Account Has Been Rejected.",
                    Type = NotificationType.Rejection,
                });
                return dto;

            }
            return null;
        }

        public PetDetailsDto? RejectPet(int id, string message)
        {
            var pet = unitOfWork.PetRepository.GetByID(id);
            var userId = unitOfWork.CustomerAddedPetsRepository.GetAllQueryable().Where(C => C.PetId == id).Select(C => C.CustomerId).FirstOrDefault();

            if (pet is not null)
            {
                //udate pet
                pet.IsDeleted = true;
                pet.IsApproved = false;
                unitOfWork.PetRepository.Update(pet);
                //add the message to database
                AdminPetMessage adminPetMessage = new AdminPetMessage()
                {
                    MessageType = AdminMessageType.Rejection,
                    PetId = id,
                    Message = message
                };
                unitOfWork.AdminPetMessageRepository.Add(adminPetMessage);
                unitOfWork.SaveChanges();
                //return the details to show in the API result 
                PetDetailsDto dto = new PetDetailsDto()
                {
                    Name = pet.Name,
                    Id = pet.Id,
                    Status = pet.Status,
                    IsApproved = pet.IsApproved,
                    IsDeleted = pet.IsDeleted
                };
                notificationService.CreateAndSendNotification(userId, new NotificationDTO()
                {
                    Message = $"Your Pet {pet.Name} With Id {pet.Id} Has Been Approved.",
                    Type = NotificationType.Approval
                });
                return dto;

            }
            return null;
        }

        public async Task<AdminStatisticsDTO> GetAdminStatistics()
        {
            //Pet Stats 
            var totalPets = await unitOfWork.PetRepository.GetAllQueryable().Where(P => P.IsDeleted == false).CountAsync();
            var totalPetsForAdpotion = await unitOfWork.PetRepository.GetAllQueryable().Where(P => P.Status == PetStatus.ForAdoption).CountAsync();
            var totalPetsForRescue = await unitOfWork.PetRepository.GetAllQueryable().Where(P => P.Status == PetStatus.ForRescue).CountAsync();
            //Users Stats
            var totalUsers = await unitOfWork.ApplicationUserRepository.GetAllQueryable().Where(U => U.IsDeleted == false).CountAsync();
            var totalDoctors = await unitOfWork.DoctorRepository.GetAllQueryable().Where(U => U.IsDeleted == false).CountAsync();
            var totalCustomers = await unitOfWork.CustomerRepository.GetAllQueryable().Where(U => U.IsDeleted == false).CountAsync();

            AdminStatisticsDTO stats = new AdminStatisticsDTO()
            {
                TotalPets = totalPets,
                PetsForAdoption = totalPetsForAdpotion,
                PetsForRescue = totalPetsForRescue,
                TotalUsers = totalUsers,
                TotalCustomers = totalCustomers,
                TotalDoctors = totalDoctors
            };
            return stats;
        }

        public CustomerDetailsDTO? GetProfile(string id)
        {
            var admin = unitOfWork.AdminRepository.GetByID(id);

            if (admin == null)
                return null;

            return new CustomerDetailsDTO
            {
                UserName = admin.UserName!,
                FName = admin.FName,
                LName = admin.LName,
                ImgUrl = admin.ImgUrl!,
                Gender = admin.Gender,
                Street = admin.Address.Street,
                City = admin.Address.City,
                Country = admin.Address.Country,
                Email = admin.Email!,
                IsApproved = admin.IsApproved,
                PhoneNumber = admin.PhoneNumber!


            };
        }

    }
}
