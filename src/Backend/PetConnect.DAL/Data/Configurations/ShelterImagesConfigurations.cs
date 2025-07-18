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
    public class ShelterImagesConfigurations : IEntityTypeConfiguration<ShelterImages>
    {
        public void Configure(EntityTypeBuilder<ShelterImages> builder)
        {
            builder.HasOne(SI => SI.Shelter).WithMany(S => S.ShelterImages).HasForeignKey(SI => SI.ShelterId);
            builder.HasKey(SI => new { SI.ShelterId, SI.ImgUrl });
            builder.Property(P => P.ImgUrl).HasColumnType("varchar(100)");
        }
    }
}
