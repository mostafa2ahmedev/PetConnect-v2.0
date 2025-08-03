using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetConnect.DAL.Data.Repositories.Classes;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.DAL.Data
{
    public static class RepositoriesCollectionExtensions
    {
        public static IServiceCollection AddDalRepositories(this IServiceCollection services)
        {
            // Repositories / Unit of Work
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<ICustomerAddedPetsRepository, CustomerAddedPetsRepository>();
            services.AddScoped<ICustomerPetAdoptionsRepository, CustomerPetAdoptionsRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPetBreedRepository, PetBreedRepository>();
            services.AddScoped<IPetCategoryRepository, PetCategoryRepository>();
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IShelterAddedPetsRepository, ShelterAddedPetsRepository>();
            services.AddScoped<IShelterImagesRepository, ShelterImagesRepository>();
            services.AddScoped<IShelterLocationsRepository, ShelterLocationsRepository>();
            services.AddScoped<IShelterOwnerRepository, ShelterOwnerRepository>();
            services.AddScoped<IShelterPetAdpotionsRepository, ShelterPetAdpotionsRepository>();
            services.AddScoped<IShelterPhonesRepository, ShelterPhonesRepository>();
            services.AddScoped<IShelterRepository, ShelterRepository>();
            services.AddScoped<ITimeSlotsRepository, TimeSlotsRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IOrderProductRepository, OrderProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
