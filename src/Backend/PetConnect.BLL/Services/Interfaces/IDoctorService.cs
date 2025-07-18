using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IDoctorService
    {
        IEnumerable<DoctorDetailsDTO> GetAll();
        DoctorDetailsDTO? GetByID(string id);
        void Add(Doctor doctor);
        void Update(DoctorDetailsDTO doctor);
        void Delete(string id);



    }
}
