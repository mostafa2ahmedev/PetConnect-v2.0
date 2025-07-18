using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetConnect.DAL.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.DAL.Data.Configurations
{
    public class DoctorConfigurations : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {

            builder.Property(D => D.PricePerHour).HasColumnType("decimal(6,2)");
            builder.Property(D => D.CertificateUrl).HasColumnType("varchar(100)");


            builder.Property(D => D.PetSpecialty)
            .HasConversion(
             PetSpecialty => PetSpecialty.ToString(),
             returnPetSpecialty => (PetSpecialty)Enum.Parse(typeof(PetSpecialty), returnPetSpecialty)

             
         );
        
        }
    }
}
