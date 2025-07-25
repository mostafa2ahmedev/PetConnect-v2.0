using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data;
using PetConnect.DAL.Data.Repositories.Classes;
using PetConnect.DAL.Data.Repositories.Interfaces;

namespace PetConnect.DAL.UnitofWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext _context)
        {
            context = _context;
        }
        public IAdminRepository AdminRepository => new AdminRepository(context);

        public ICustomerRepository CustomerRepository => new CustomerRepository(context);

        public ICustomerAddedPetsRepository CustomerAddedPetsRepository => new CustomerAddedPetsRepository(context);

        public ICustomerPetAdoptionsRepository CustomerPetAdpotionsRepository => new CustomerPetAdoptionsRepository(context);

        public IDoctorRepository DoctorRepository => new DoctorRepository(context);

        public IPetRepository PetRepository => new PetRepository(context);

        public IPetBreedRepository PetBreedRepository => new PetBreedRepository(context);

        public IPetCategoryRepository PetCategoryRepository => new PetCategoryRepository(context);

        public IShelterRepository ShelterRepository => new ShelterRepository(context);

        public IShelterOwnerRepository ShelterOwnerRepository => new ShelterOwnerRepository(context);

        public IShelterAddedPetsRepository ShelterAddedPetsRepository => new ShelterAddedPetsRepository(context);

        public IShelterPetAdpotionsRepository ShelterPetAdpotionsRepository => new ShelterPetAdpotionsRepository(context);

        public IShelterImagesRepository ShelterImagesRepository => new ShelterImagesRepository(context);

        public IShelterLocationsRepository ShelterLocationsRepository => new ShelterLocationsRepository(context);

        public IShelterPhonesRepository ShelterPhonesRepository => new ShelterPhonesRepository(context);

        public IProductRepository ProductRepository => new ProductRepository(context);
        public IProductTypeRepository ProductTypeRepository => new ProductTypeRepository(context);
        public IOrderRepository OrderRepository => new OrderRepository(context);

        public ITimeSlotsRepository TimeSlotsRepository =>  new TimeSlotsRepository(context);

        public void Dispose()
        {
            context.Dispose();
        }
        public int SaveChanges()
        {
            return context.SaveChanges();
        }
    }
}
