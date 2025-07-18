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
    public class CustomerAddedPetsConfigurations : IEntityTypeConfiguration<CustomerAddedPets>
    {
        public void Configure(EntityTypeBuilder<CustomerAddedPets> builder)
        {

            builder.HasKey(CAP => new { CAP.PetId, CAP.CustomerId }); // Composite key

            builder.HasOne(CAP => CAP.Customer).WithMany(S => S.CustomerAddedPets).HasForeignKey(CAP => CAP.CustomerId);

            builder.HasOne(CAP => CAP.Pet).WithOne(C => C.CustomerAddedPets).HasForeignKey<CustomerAddedPets>(CAP => CAP.PetId);

            
            builder.Property(C => C.Status)
            .HasConversion(
            PetStatus => PetStatus.ToString(),
            AddedStatus => (AddedStatus)Enum.Parse(typeof(AddedStatus), AddedStatus)
                );
        }
    }
}
