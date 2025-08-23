using Microsoft.AspNetCore.Builder;
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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(d => d.Id).UseIdentityColumn(1, 1);
            builder.Property(d => d.Name).IsRequired().HasColumnType("varchar(50)");
            builder.Property(d => d.ImgUrl).HasMaxLength(255);
            builder.Property(d => d.Description).HasColumnType("varchar(150)");
            builder.Property(d => d.Price).HasColumnType("decimal(18,2)");
            builder.HasOne(p => p.Producttype).WithMany(d => d.Products).HasForeignKey(d => d.ProductTypeId);

            builder.HasOne(p => p.Seller).WithMany(S => S.AddedProducts).HasForeignKey(P => P.SellerId);
        }
    }
}
