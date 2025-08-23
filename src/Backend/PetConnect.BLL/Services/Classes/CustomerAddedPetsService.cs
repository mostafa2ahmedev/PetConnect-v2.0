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
    public class CustomerAddedPetsService : ICustomerAddedPetsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerAddedPetsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void RegisterCustomerPetAddition(string userId, int petId)
        {
            var CustomerPetAddition = new CustomerAddedPets()
            {
                CustomerId = userId,
                PetId = petId,
                AdditionDate = DateTime.Now,
                Status = AddedStatus.Pending,
            };
            var pet = _unitOfWork.PetRepository.GetByID(petId);
            //pet.Status = PetStatus.Owned;
            _unitOfWork.CustomerAddedPetsRepository.Add(CustomerPetAddition);
        }
    }
}
