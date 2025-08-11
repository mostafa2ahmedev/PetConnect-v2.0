using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Classes;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.UnitofWork;
using System.Collections.Generic;
using System.Linq;

namespace PetConnect.BLL.Services.Classes
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork UOW;


        public DoctorService(IUnitOfWork _UOW )
        {
            UOW = _UOW;
        }
        
        public IEnumerable<DoctorDetailsDTO> GetAll()
        {
            return UOW.DoctorRepository.GetAll()
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
                    City = d.Address.City
                });
        }

       
        public DoctorDetailsDTO? GetByID(string id)
        {
            var doctor = UOW.DoctorRepository.GetByID(id);
            if (doctor == null || !doctor.IsApproved)
                return null;

            return new DoctorDetailsDTO
            {
                Id = doctor.Id,
                FName = doctor.FName,
                LName = doctor.LName,
                ImgUrl = doctor.ImgUrl ?? "/assets/img/default-doctor.jpg",
                PetSpecialty = doctor.PetSpecialty.ToString(),
                Gender = doctor.Gender.ToString(),
                PricePerHour = doctor.PricePerHour,
                CertificateUrl = doctor.CertificateUrl,
                Street = doctor.Address.Street,
                City = doctor.Address.City
            };
        }

        public void Add(Doctor doctor)
        {
            UOW.DoctorRepository.Add(doctor);
            UOW.SaveChanges();
        }

        public void Update(DoctorDetailsDTO dto)
        {
            var doctor = UOW.DoctorRepository.GetByID(dto.Id);

            if (doctor == null || !doctor.IsApproved)
                throw new Exception("Doctor not found");

         
            doctor.FName = dto.FName;
            doctor.LName = dto.LName;
            doctor.ImgUrl = dto.ImgUrl;
            doctor.PricePerHour = dto.PricePerHour;
            doctor.CertificateUrl = dto.CertificateUrl;

           
            if (Enum.TryParse(dto.PetSpecialty, out PetSpecialty specialty))
                doctor.PetSpecialty = specialty;

            if (Enum.TryParse(dto.Gender, out Gender gender))
                doctor.Gender = gender;

          
            if (doctor.Address == null)
                doctor.Address = new Address();

            doctor.Address.Street = dto.Street;
            doctor.Address.City = dto.City;

            
            UOW.DoctorRepository.Update(doctor);
            UOW.SaveChanges();
        }

        public void Delete(string id)
        {
            var doctor = UOW.DoctorRepository.GetByID(id);
            if(doctor is not null)
            {
                UOW.DoctorRepository.Delete(doctor);
                UOW.SaveChanges();
            }

        }
    }
}
