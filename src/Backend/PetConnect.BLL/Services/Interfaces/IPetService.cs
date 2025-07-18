
using Microsoft.AspNetCore.Http;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IPetService
    {
        

       Task <int> AddPet(AddedPetDto addedPet );
        void UpdatePet(UpdatedPetDto UpdatedPet);
        void DeletePet(int id);

        IEnumerable<PetDataDto> GetAllPets();
        PetDetailsDto? GetPet(int id);
        IEnumerable<PetDataDto> GetAllPetsByCountForAdoption(int count);

    }
}
