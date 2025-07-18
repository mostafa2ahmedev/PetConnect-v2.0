using PetConnect.BLL.Services.DTOs.Customer;
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
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPetService _petService;

        public CustomerService(IUnitOfWork unitOfWork,IPetService petService)
        {
            _unitOfWork = unitOfWork;
            _petService = petService;
        }

 

        public void RequestAdoption(CusRequestAdoptionDto adoptionDto)
        {
            var CusReqAdoption = new CustomerPetAdoptions() {
            CustomerId = adoptionDto.CustomerId,
            PetId = adoptionDto.PetId,  
            Status =AdoptionStatus.Pending,
            AdoptionDate = DateTime.Now,
            };
            _unitOfWork.CustomerPetAdpotionsRepository.Add(CusReqAdoption);
            _unitOfWork.SaveChanges();
        }
        public List<DetailsCustomerRequestAdoption> GetCustomerReqAdoptionsData(string userId)
        {
            var CustomerReqAdoptionsData = _unitOfWork.CustomerPetAdpotionsRepository.GetAll().Where(CPA => CPA.CustomerId == userId).ToList();

           List< DetailsCustomerRequestAdoption> detailsCustomerRequestAdoption = new List< DetailsCustomerRequestAdoption>() { };
       
            foreach (var item in CustomerReqAdoptionsData)
            {
                var Pet = _unitOfWork.PetRepository.GetByID(item.PetId);
                var Bread = _unitOfWork.PetBreedRepository.GetByID(Pet.BreedId);
                var Category = _unitOfWork.PetCategoryRepository.GetByID(Bread.CategoryId);

                detailsCustomerRequestAdoption.Add(new DetailsCustomerRequestAdoption() {
                    AdoptionDate = item.AdoptionDate,
                    Status = item.Status,   
                    PetName = Pet.Name,
                    PetBreadName=Bread.Name,
                    PetCategoryName=Category.Name
                });
            }

            return detailsCustomerRequestAdoption;
        }

    }
}
