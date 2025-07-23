using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetConnect.DAL.Data.Models;

namespace PetConnect.DAL.Data.Configurations
{
    class TimeSlotConfigurations : IEntityTypeConfiguration<TimeSlot>
    {

        public void Configure(EntityTypeBuilder<TimeSlot> builder)
        {
            builder.HasOne(ts => ts.Doctor)
                .WithMany(d => d.TimeSlots)
                .HasForeignKey(ts => ts.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ts => ts.StartTime)
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(ts => ts.EndTime)
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.HasMany(ts => ts.Appointments)
              .WithOne(a => a.TimeSlot)
              .HasForeignKey(a => a.SlotId);

            builder.Ignore(t => t.IsFull);

        }
    }
}
