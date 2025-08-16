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
    public class SupportRequestConfiguration : IEntityTypeConfiguration<SupportRequest>
    {
        public void Configure(EntityTypeBuilder<SupportRequest> builder)
        {

            builder.HasKey(SR => SR.Id);
         
            builder.Property(SR => SR.Message).HasColumnType("varchar(500)");
            builder.Property(SR => SR.PictureUrl).HasColumnType("varchar(200)");
            builder.Property(SR => SR.Type)
             .HasConversion(
             Type => Type.ToString(),
             returnType => (SupportRequestType)Enum.Parse(typeof(SupportRequestType), returnType)
             );
            builder.Property(SR => SR.Status)
             .HasConversion(
             Status => Status.ToString(),
             returnStatus => (SupportRequestStatus)Enum.Parse(typeof(SupportRequestStatus), returnStatus)
             );

            builder.Property(SR => SR.Priority)
             .HasConversion(
             Priority => Priority.ToString(),
             returnPriority => (SupportRequestPriority)Enum.Parse(typeof(SupportRequestPriority), returnPriority)
             );

            builder.HasOne(SR => SR.User).WithMany(U => U.SupportRequests).HasForeignKey(SR => SR.UserId).OnDelete(DeleteBehavior.NoAction);


            builder.Property(SR => SR.CreatedAt).HasDefaultValueSql("GetDate()");

        }
    }
}
