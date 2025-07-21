
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Identity;
using PetConnect.DAL.Data;
using PetConnect.DAL.Services;

namespace PetConnect.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string AngularCorsPolicy = "_angularCorsPolicy";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer("Server=db22694.public.databaseasp.net; Database=db22694; User Id=db22694; Password=Qf9?z@N38k!P; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");
            });
            //Repositories Services register
            RepositoriesCollectionExtensions.AddDalRepositories(builder.Services);
            ServicesCollectionExtensions.AddBLLRepositories(builder.Services);



            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AngularCorsPolicy,
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:4200")  // Allow only Angular dev server origin
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();


            app.UseCors(AngularCorsPolicy);

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
