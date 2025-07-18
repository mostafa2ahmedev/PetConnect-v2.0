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
    public class PetCategoryConfigurations : IEntityTypeConfiguration<PetCategory>
    {
        public void Configure(EntityTypeBuilder<PetCategory> builder)
        {

            builder.Property(P => P.Name).HasColumnType("varchar(20)");


        }
    }
}
