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
    internal class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(x => x.Id);  

            builder.Property(n => n.CreatedAt)
             .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(P => P.NotificationType)
               .HasConversion(
               NotificationType => NotificationType.ToString(),
               returnNotificationType => (NotificationType)Enum.Parse(typeof(NotificationType), returnNotificationType)
               );
        }
    }
}
