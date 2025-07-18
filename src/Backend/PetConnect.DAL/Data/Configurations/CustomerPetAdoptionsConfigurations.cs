using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Configurations
{
    public class CustomerPetAdoptionsConfigurations : IEntityTypeConfiguration<CustomerPetAdoptions>
    {
        public void Configure(EntityTypeBuilder<CustomerPetAdoptions> builder)
        {
            builder.HasKey(CPA => new { CPA.PetId, CPA.CustomerId });
            builder.HasOne(CPA => CPA.Customer).WithMany(S => S.CustomerPetAdoptions)
                .HasForeignKey(CPA => CPA.CustomerId);

            builder.HasOne(CPA => CPA.Pet).WithMany(S => S.CustomerPetAdoptions)
                .HasForeignKey(CPA => CPA.PetId);


            builder.Property(C => C.Status)
            .HasConversion(
            PetStatus => PetStatus.ToString(),
            AdoptionStatus => (AdoptionStatus)Enum.Parse(typeof(AdoptionStatus), AdoptionStatus)
            );
        }
    }
}
