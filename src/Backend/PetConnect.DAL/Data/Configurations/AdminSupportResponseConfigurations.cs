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
    public class AdminSupportResponseConfigurations : IEntityTypeConfiguration<AdminSupportResponse>
    {
        public void Configure(EntityTypeBuilder<AdminSupportResponse> builder)
        {

            builder.HasKey(ASR => ASR.Id);
         
            builder.Property(ASR => ASR.Message).HasColumnType("varchar(max)");
            builder.Property(ASR => ASR.PictureUrl).HasColumnType("varchar(200)");

            builder.HasOne(ASR => ASR.Request).WithMany(R => R.AdminSupportResponses).HasForeignKey(SR => SR.SupportRequestId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(ASR => ASR.CreatedAt).HasDefaultValueSql("GetDate()");
        }
    }
}
