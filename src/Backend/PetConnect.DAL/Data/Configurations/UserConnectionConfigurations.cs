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
    public class UserConnectionConfigurations : IEntityTypeConfiguration<UserConnection>
    {
        public void Configure(EntityTypeBuilder<UserConnection> builder)
        {
            builder.HasKey(C => C.ConnectionId);
            builder.HasOne(C => C.ApplicationUser)
                .WithOne(U => U.UserConnection)
                .HasForeignKey<UserConnection>(U => U.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
