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
    public class ShelterAddedPetsConfigurations : IEntityTypeConfiguration<ShelterAddedPets>
    {
        public void Configure(EntityTypeBuilder<ShelterAddedPets> builder)
        {


            builder.HasKey(SAP => new { SAP.PetId, SAP.ShelterId }); // Composite key

         builder.HasOne(SAP => SAP.Shelter).WithMany(S => S.ShelterAddedPets).HasForeignKey(SAP => SAP.ShelterId);

         builder.HasOne(SAP => SAP.Pet).WithOne(P =>P.ShelterAddedPets ).HasForeignKey<ShelterAddedPets>(SAP =>SAP.PetId);
           
            builder.Property(E => E.Status)
                 .HasConversion(
                     PetStatus => PetStatus.ToString(),
                     AddedStatus => (AddedStatus)Enum.Parse(typeof(AddedStatus), AddedStatus)
                    );


        }
    }
}
