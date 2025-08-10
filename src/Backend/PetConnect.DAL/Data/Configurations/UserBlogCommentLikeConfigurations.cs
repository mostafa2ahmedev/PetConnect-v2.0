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
    public class UserBlogCommentLikeConfigurations : IEntityTypeConfiguration<UserBlogCommentLike>
    {
        public void Configure(EntityTypeBuilder<UserBlogCommentLike> builder)
        {
            builder.HasOne(UBCL => UBCL.BlogComment).WithMany(BC => BC.UserBlogCommentLikes).HasForeignKey(UBCL => UBCL.BlogCommentId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(UBCL => UBCL.User).WithMany(U => U.UserBlogCommentLikes).HasForeignKey(UBCL => UBCL.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasKey(UBCL => new { UBCL.BlogCommentId, UBCL.UserId });

        }
    }
}
