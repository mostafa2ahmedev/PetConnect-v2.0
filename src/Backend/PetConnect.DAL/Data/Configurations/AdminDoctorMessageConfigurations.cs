using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;

namespace PetConnect.DAL.Data.Configurations
{
    public class AdminDoctorMessageConfigurations : IEntityTypeConfiguration<AdminPetMessage>
    {
        public void Configure(EntityTypeBuilder<AdminPetMessage> builder)
        {
            builder.Property(a => a.MessageType)
                .HasConversion(
                status => status.ToString(),
                  value => (AdminMessageType)Enum.Parse(typeof(AdminMessageType), value))
                .HasColumnType("varchar(20)");

            builder.Property(a => a.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()")
               .ValueGeneratedOnAdd();

            builder.HasOne(m => m.Pet)
                .WithMany(d => d.AdminPetMessages)
                .HasForeignKey(m => m.PetId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
