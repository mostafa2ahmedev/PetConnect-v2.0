using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Identity;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Configurations
{
    public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.Property(AU=>AU.FName).HasColumnType("varchar(20)");
            builder.Property(AU => AU.LName).HasColumnType("varchar(20)");
            builder.Property(AU => AU.ImgUrl).HasColumnType("varchar(100)");

            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.Street)
                       .HasColumnName("Street")
                 .HasColumnType("varchar(20)");

                address.Property(a => a.City)
                       .HasColumnName("City")
                 .HasColumnType("varchar(20)");

                address.Property(a => a.Country)
                       .HasColumnName("Country")
                   .HasColumnType("varchar(20)");
            });

            builder.Property(AU => AU.Gender)
            .HasConversion(
             GenderStatus => GenderStatus.ToString(),
             returnGender => (Gender)Enum.Parse(typeof(Gender), returnGender)
         );
        }
    }
}
