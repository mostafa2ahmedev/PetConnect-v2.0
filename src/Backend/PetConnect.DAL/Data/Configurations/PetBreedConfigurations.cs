using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Configurations
{
    public class PetBreedConfigurations : IEntityTypeConfiguration<PetBreed>
    {
        public void Configure(EntityTypeBuilder<PetBreed> builder)
        {
            builder.Property(P => P.Name).HasColumnType("varchar(20)");
            builder.HasOne(PB => PB.Category).WithMany(PC => PC.Breeds).HasForeignKey(PB => PB.CategoryId);



        }
    }
}
