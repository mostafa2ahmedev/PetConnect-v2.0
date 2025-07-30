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
        public IUserRepository UserRepository { get; }
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
        public ITimeSlotsRepository TimeSlotsRepository { get; }
        public IAppointmentsRepository AppointmentsRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IProductTypeRepository ProductTypeRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public ITimeSlotsRepository TimeSlotsRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IUserMessagesRepository UserMessagesRepository { get; }
        public IUserConnectionRepository UserConnectionRepository { get; }
        public IAdminDoctorMessageRepository AdminDoctorMessageRepository { get;  }
        public IAdminPetMessageRepository AdminPetMessageRepository { get;  }
        public IApplicationUserRepository ApplicationUserRepository{ get;  }
        public int SaveChanges();
        Task<int> SaveChangesAsync();

    }

}
