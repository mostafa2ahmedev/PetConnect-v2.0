using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetConnect.DAL.Data.Models;
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
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IOrderProductRepository, OrderProductRepository>();
            services.AddScoped<ITimeSlotsRepository, TimeSlotsRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserConnectionRepository, UserConnectionRepository>();
            services.AddScoped<IUserMessagesRepository, UserMessagesRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IBlogCommentRepository, BlogCommentRepository>();
            services.AddScoped<IBlogCommentReplyRepository, BlogCommentReplyRepository>();
            services.AddScoped<IUserBlogLikeRepository, UserBlogLikeRepository>();
            services.AddScoped<IUserBlogCommentLikeRepository, UserBlogCommentLikeRepository>();
            services.AddScoped<IUserBlogCommentRepository, UserBlogCommentRepository>();
            services.AddScoped<IUserBlogCommentReplyLikeRepository, UserBlogCommentReplyLikeRepository>();
            services.AddScoped<IUserBlogCommentReplyRepository, UserBlogCommentReplyRepository>();

            return services;
        }
    }
}
