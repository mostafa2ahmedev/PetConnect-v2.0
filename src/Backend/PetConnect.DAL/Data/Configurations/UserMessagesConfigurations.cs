using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;

namespace PetConnect.DAL.Data.Configurations
{
    public class UserMessagesConfigurations : IEntityTypeConfiguration<UsersMessages>
    {
        public void Configure(EntityTypeBuilder<UsersMessages> builder)
        {
            builder.HasKey(M => M.MessageId);
            builder.Property(a => a.MessageType)
                   .HasConversion(status => status.ToString(),
                    value => (UserMessageType)Enum.Parse(typeof(UserMessageType), value))
                    .HasColumnType("varchar(20)");

            builder.HasOne(M => M.Sender)
                .WithMany(U => U.SentMessages)
                .HasForeignKey(M => M.SenderId).
                OnDelete(DeleteBehavior.Restrict);



            builder.HasOne(M => M.Receiver)
                .WithMany(U => U.ReceivedMessages)
                .HasForeignKey(M => M.RecieverId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Property(M => M.ReadDate)
                .HasColumnType("datetime2(0)");


            builder.Property(a => a.SentDate)
                   .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAdd();
        }
    }
}
