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
    public class OrderProductConfigurations : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(op => new { op.OrderId, op.ProductId });
            builder.HasOne(o => o.order).WithMany(o => o.OrderProducts).HasForeignKey(o => o.OrderId);
            builder.HasOne(p => p.product).WithMany(o => o.OrderProducts).HasForeignKey(o => o.ProductId);
            builder.Property(op => op.Quantity).IsRequired();
            builder.Property(op => op.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
        }
    }
}
