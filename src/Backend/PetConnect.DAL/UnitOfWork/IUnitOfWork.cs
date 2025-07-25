using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Repositories.Classes;
using PetConnect.DAL.Data.Repositories.Interfaces;

namespace PetConnect.DAL.UnitofWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IAdminRepository AdminRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public ICustomerAddedPetsRepository CustomerAddedPetsRepository { get; }
        public ICustomerPetAdoptionsRepository CustomerPetAdpotionsRepository { get; }
        public IDoctorRepository DoctorRepository { get; }
        public IPetRepository PetRepository { get; }
        public IPetBreedRepository PetBreedRepository { get; }
        public IPetCategoryRepository PetCategoryRepository { get; }
        public IShelterRepository ShelterRepository { get; }
        public IShelterOwnerRepository ShelterOwnerRepository { get; }
        public IShelterAddedPetsRepository ShelterAddedPetsRepository { get; }
        public IShelterPetAdpotionsRepository ShelterPetAdpotionsRepository { get; }
        public IShelterImagesRepository ShelterImagesRepository { get; }
        public IShelterLocationsRepository ShelterLocationsRepository { get; }
        public IShelterPhonesRepository ShelterPhonesRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IProductTypeRepository ProductTypeRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public ITimeSlotsRepository TimeSlotsRepository { get; }
        public IAdoptionNotificationRepository AdoptionNotificationRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public int SaveChanges();
    }

}
