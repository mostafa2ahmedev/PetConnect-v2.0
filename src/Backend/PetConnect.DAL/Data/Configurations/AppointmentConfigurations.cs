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
    public class AppointmentConfigurations : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.Property(a => a.Status)
                   .HasConversion(
                        status => status.ToString(),
                        value => (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), value))
                   .HasColumnType("varchar(20)").HasDefaultValue(AppointmentStatus.Confirmed);


            builder.Property(a => a.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAdd();

            builder.HasOne(a => a.TimeSlot)
                   .WithMany(t => t.Appointments)
                   .HasForeignKey(a => a.SlotId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Doctor)
                   .WithMany(d => d.Appointments)
                   .HasForeignKey(a => a.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Customer)
                   .WithMany(c => c.Appointments)
                   .HasForeignKey(a => a.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Pet)
                .WithMany()
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
