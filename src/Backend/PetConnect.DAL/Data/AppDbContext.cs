using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetConnect.DAL.Data.Identity;
using PetConnect.DAL.Data.Models;
using System.Reflection;


namespace PetConnect.DAL.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {

        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetBreed> PetBreeds { get; set; }
        public DbSet<PetCategory> PetCategory { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<ShelterAddedPets> ShelterAddedPets { get; set; }
        public DbSet<ShelterPetAdoptions> ShelterPetAdoptions { get; set; }
        public DbSet<CustomerAddedPets> CustomerAddedPets { get; set; }
        public DbSet<CustomerPetAdoptions> CustomerPetAdoptions { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        
        public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions)
        {
            
        }

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            builder.Entity<Customer>().ToTable("Customers");         
            builder.Entity<Doctor>().ToTable("Doctors");
            builder.Entity<ShelterOwner>().ToTable("ShelterOwners");
            builder.Entity<Admin>().ToTable("Admins");

        }

    }
}
