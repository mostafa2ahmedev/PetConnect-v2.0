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
    public class PetConfigurations : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.Property(P => P.Name).HasColumnType("varchar(20)");
            builder.Property(P => P.ImgUrl).HasColumnType("varchar(100)");
            builder.Property(AU => AU.IsDeleted).HasDefaultValue(false);



            builder.HasOne(P => P.Breed).WithMany(PB=>PB.Pets).HasForeignKey(P=>P.BreedId);

             builder.Property(P => P.Status)
             .HasConversion(
              PetStatus => PetStatus.ToString(),
             returnStatus => (PetStatus)Enum.Parse(typeof(PetStatus), returnStatus)
            );

            builder.Property(P => P.Ownership)
                .HasConversion(
                Ownership => Ownership.ToString(),
                returnOwnership => (Ownership)Enum.Parse(typeof(Ownership), returnOwnership)
                );
        }
    }
}
