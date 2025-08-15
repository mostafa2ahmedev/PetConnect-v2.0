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
    public class DoctorReviewConfigurations : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {

            builder.Property(r => r.Rating)
                   .IsRequired();


            builder.Property(r => r.ReviewText)
                   .HasColumnType("nvarchar(max)");


            builder.Property(r => r.ReviewDate)
                   .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAdd();


            builder.HasOne(r => r.Doctor)
                   .WithMany(d => d.DoctorReviews)
                   .HasForeignKey(r => r.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(r => r.Customer)
                   .WithMany(c => c.DoctorReviews)
                   .HasForeignKey(r => r.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(r => new { r.DoctorId, r.CustomerId });

        }
    }


}
