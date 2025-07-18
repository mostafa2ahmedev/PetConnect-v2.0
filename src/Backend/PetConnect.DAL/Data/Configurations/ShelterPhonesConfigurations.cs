using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Configurations
{
    public class ShelterPhonesConfigurations : IEntityTypeConfiguration<ShelterPhones>
    {
        public void Configure(EntityTypeBuilder<ShelterPhones> builder)
        {
            builder.Property(P => P.Phone).HasColumnType("varchar(15)");
            builder.HasOne(SL => SL.Shelter).WithMany(S => S.ShelterPhones).HasForeignKey(SL => SL.ShelterId);
            builder.HasKey(SL => new { SL.ShelterId, SL.Phone });
        }
    }
}
