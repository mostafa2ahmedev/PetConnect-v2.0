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
    public class ShelterLocationsConfigurations : IEntityTypeConfiguration<ShelterLocations>
    {
        public void Configure(EntityTypeBuilder<ShelterLocations> builder)
        {
            builder.HasOne(SL => SL.Shelter).WithMany(S => S.ShelterLocations).HasForeignKey(SL => SL.ShelterId);
            builder.HasKey(SL => new { SL.ShelterId, SL.LocationCode });


            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.Street)
                       .HasColumnName("Street")
                       .HasColumnType("varchar(20)");

                address.Property(a => a.City)
                       .HasColumnName("City")
                            .HasColumnType("varchar(20)");

                address.Property(a => a.Country)
                       .HasColumnName("Country")
                                 .HasColumnType("varchar(20)");
            });
        }
    }
}
