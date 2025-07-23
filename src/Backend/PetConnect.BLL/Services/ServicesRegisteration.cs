using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Repositories.Classes;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.DAL.Services
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddBLLRepositories(this IServiceCollection services)
        {
            // Services 

            services.AddScoped<IPetCategoryService, PetCategoryService>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IPetBreadService,PetBreadService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICustomerAddedPetsService, CustomerAddedPetsService>();
            services.AddScoped<ICustomerService, CustomerService>();
            return services;
        }
    }
}
