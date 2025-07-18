using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Admin;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.BLL.Services.Classes
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork unitOfWork;

        public AdminService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        //public IEnumerable<DoctorDetailsDTO> GetPendingDoctors() 
        //{
        //    return unitOfWork.DoctorRepository.GetAll()
        //        .Select(d => new DoctorDetailsDTO
        //        {
        //            Id = d.Id,
        //            FName = d.FName,
        //            LName = d.LName,
        //            ImgUrl = d.ImgUrl ?? "/assets/img/default-doctor.jpg",
        //            PetSpecialty = d.PetSpecialty.ToString(),
        //            Gender = d.Gender.ToString(),
        //            PricePerHour = d.PricePerHour,
        //            CertificateUrl = d.CertificateUrl,
        //            Street = d.Address.Street,
        //            City = d.Address.City,
        //            IsApproved = d.IsApproved
        //        }).Where(D => D.IsApproved == false);
        //}

        //public IEnumerable<AddedPetDto> GetPendingPets()
        //{
        //    return unitOfWork.PetRepository.GetAll()
        //        .Select(d => new AddedPetDto
        //        {
        //            Name = d.Name,
        //            Status=d.Status,
        //            IsApproved =d.IsApproved,
        //            Ownership=d.Ownership,
        //            BreedId=d.BreedId
        //        }).Where(P => P.IsApproved == false);
        //}
        public AdminDashboardDTO GetPendingDoctorsAndPets()
        {
            var pendingDoctors = unitOfWork.DoctorRepository.GetAll()
                .Where(d => !d.IsApproved)
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
                    IsApproved = d.IsApproved
                }).ToList();

            var pendingPets = unitOfWork.PetRepository.GetPendingPetsWithBreedAndCategory()
                .Where(p => !p.IsApproved)
                .Select(p => new PetDetailsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Status = p.Status,
                    IsApproved = p.IsApproved,
                    Ownership = p.Ownership,
                    ImgUrl = p.ImgUrl ?? "/assets/img/default-doctor.jpg",
                    BreadName = p.Breed.Name,
                    CategoryName = p.Breed.Category.Name
                }).ToList();

            return new AdminDashboardDTO
            {
                PendingDoctors = pendingDoctors,
                PendingPets = pendingPets
            };
        }

        public void ApproveDoctor(string id) 
        {
            Doctor? doctor =unitOfWork.DoctorRepository.GetByID(id);
            if (doctor is not null)
            {
                doctor.IsApproved = true;
                unitOfWork.DoctorRepository.Update(doctor);
                unitOfWork.SaveChanges();
            }
        }

        public void ApprovePet(int id)
        {
            Pet? pet = unitOfWork.PetRepository.GetByID(id);
            if (pet is not null)
            {
                pet.IsApproved = true;
                unitOfWork.PetRepository.Update(pet);
                unitOfWork.SaveChanges();
            }
        }

    }
}
