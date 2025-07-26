using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Configurations
{
    internal class AdoptionNotificationConfigurations : IEntityTypeConfiguration<AdoptionNotification>
    {
        public void Configure(EntityTypeBuilder<AdoptionNotification> builder)
        {
            builder.HasKey(AN => AN.NotificationId);
            builder.HasOne(AN => AN.Notification).WithOne(N=>N.AdoptionNotification).HasForeignKey<AdoptionNotification>(AN=>AN.NotificationId);
            builder.HasOne(AN => AN.Customer).WithMany(C=>C.AdoptionNotifications).HasForeignKey(c=>c.CustomerId);
        }
    }
}
