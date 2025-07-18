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
    public class ShelterPetAdoptionsConfigurations : IEntityTypeConfiguration<ShelterPetAdoptions>
    {
        public void Configure(EntityTypeBuilder<ShelterPetAdoptions> builder)
        {
            builder.HasKey(SPA => new { SPA.PetId, SPA.ShelterId });
            builder.HasOne(SPA => SPA.Shelter).WithMany(S => S.ShelterPetAdoptions)
                .HasForeignKey(SPA=>SPA.ShelterId);

            builder.HasOne(SPA => SPA.Pet).WithMany(S => S.ShelterPetAdoptions)
                .HasForeignKey(SPA => SPA.PetId);

            builder.Property(S => S.Status)
        .HasConversion(
         PetStatus => PetStatus.ToString(),
         AdoptionStatus => (AdoptionStatus)Enum.Parse(typeof(AdoptionStatus), AdoptionStatus)
        );

        }
    }
}
