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
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
            builder.HasOne(o => o.customer).WithMany(o => o.Orders).HasForeignKey(o => o.CustomerId);
            builder.Property(O => O.OrderStatus)
              .HasConversion(
              OrderStatus => OrderStatus.ToString(),
              returnOrderStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), returnOrderStatus)
              );


            builder.OwnsOne(u => u.ShippingAddress, ShippingAddress =>
            {
                ShippingAddress.Property(a => a.Street)
                       .HasColumnName("Street")
                 .HasColumnType("varchar(20)");

                ShippingAddress.Property(a => a.City)
                       .HasColumnName("City")
                 .HasColumnType("varchar(20)");

                ShippingAddress.Property(a => a.Country)
                       .HasColumnName("Country")
                   .HasColumnType("varchar(20)");
            });

            builder.HasOne(o => o.DeliveryMethod).WithOne().HasForeignKey<Order>(o => o.DeliveryMethodId);
        }
    }
}
