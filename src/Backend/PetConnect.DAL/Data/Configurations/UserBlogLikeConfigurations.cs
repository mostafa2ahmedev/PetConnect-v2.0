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
    public class UserBlogLikeConfigurations : IEntityTypeConfiguration<UserBlogLike>
    {
        public void Configure(EntityTypeBuilder<UserBlogLike> builder)
        {
            builder.HasOne(UBL => UBL.Blog).WithMany(B => B.UserBlogLikes).HasForeignKey(UBL => UBL.BlogId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(UBL => UBL.User).WithMany(U => U.UserBlogLikes).HasForeignKey(UBL => UBL.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasKey(UBL => new { UBL.BlogId, UBL.UserId });

        }
    }
}
