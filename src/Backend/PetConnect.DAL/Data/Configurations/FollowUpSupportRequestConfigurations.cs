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
    public class FollowUpSupportRequestConfigurations : IEntityTypeConfiguration<FollowUpSupportRequest>
    {
        public void Configure(EntityTypeBuilder<FollowUpSupportRequest> builder)
        {
            builder.HasKey(FUSR => FUSR.Id);

            builder.Property(FUSR => FUSR.Message).HasColumnType("varchar(max)");
            builder.Property(FUSR => FUSR.PictureUrl).HasColumnType("varchar(200)");

            builder.HasOne(FUSR => FUSR.Request).WithMany(R => R.FollowUpSupportRequests).HasForeignKey(SR => SR.SupportRequestId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(FUSR => FUSR.CreatedAt).HasDefaultValueSql("GetDate()");
        }
    }
}
