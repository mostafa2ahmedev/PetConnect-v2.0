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
    public class SupportResponseConfigurations : IEntityTypeConfiguration<SupportResponse>
    {
        public void Configure(EntityTypeBuilder<SupportResponse> builder)
        {

            builder.HasKey(SR => SR.Id);
         
            builder.Property(SR => SR.Message).HasColumnType("varchar(max)");
           
    
            builder.HasOne(SR => SR.Request).WithMany(R => R.SupportResponses).HasForeignKey(SR => SR.SupportRequestId).OnDelete(DeleteBehavior.Cascade);
    
        }
    }
}
