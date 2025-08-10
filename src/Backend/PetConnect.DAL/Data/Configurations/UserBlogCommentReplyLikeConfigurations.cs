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
    public class UserBlogCommentReplyLikeConfigurations : IEntityTypeConfiguration<UserBlogCommentReplyLike>
    {
        public void Configure(EntityTypeBuilder<UserBlogCommentReplyLike> builder)
        {
            builder.HasOne(UBCRL => UBCRL.BlogCommentReply).WithMany(BCR => BCR.UserBlogCommentReplyLikes).HasForeignKey(UBLR => UBLR.BlogCommentReplyId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(UBCRL => UBCRL.User).WithMany(U => U.UserBlogCommentReplyLikes).HasForeignKey(UBLR => UBLR.UserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasKey(UBCRL => new { UBCRL.BlogCommentReplyId, UBCRL.UserId });
        }
    }
}
