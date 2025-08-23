
using Microsoft.AspNetCore.Http;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.DAL.Data.Enums;
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


        Task <int> AddPet(AddedPetDto addedPet ,string CustomerId);

        Task<int> UpdatePet(UpdatedPetDto UpdatedPet);
        public int UpdatePetStatus(int petId, PetStatus newStatus);

        int DeletePet(int id);
        IEnumerable<PetDataDto> GetAllPetsWithCustomerData();
        //IEnumerable<PetDataDto> GetAllPets();

        public IEnumerable<PetDataDto> GetAllApprovedPetsWithCustomerData();

        PetDetailsDto? GetPet(int id);
        IEnumerable<PetDataDto> GetAllPetsByCountForAdoption(int count);

        IEnumerable<PetDetailsDto> GetPetsForCustomer(string CustomerId);

        public IEnumerable<PetDataDto> GetAllForAdoptionPetsWithCustomerData();
        public IEnumerable<PetDataDto> GetAllForRescuePetsWithCustomerData();

    }
}
