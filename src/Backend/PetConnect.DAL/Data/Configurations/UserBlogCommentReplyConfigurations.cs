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
    public class UserBlogCommentReplyConfigurations : IEntityTypeConfiguration<UserBlogCommentReply>
    {
        public void Configure(EntityTypeBuilder<UserBlogCommentReply> builder)
        {
            builder.HasOne(UBCR => UBCR.BlogComment).WithMany(BC => BC.UserBlogCommentReplies).HasForeignKey(UBCR => UBCR.BlogCommentId).OnDelete(DeleteBehavior.NoAction); ;
            builder.HasOne(UBCR => UBCR.User).WithMany(U => U.UserBlogCommentReplies).HasForeignKey(UBCR => UBCR.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(UBCR => UBCR.BlogCommentReply).WithOne(BCR => BCR.UserBlogCommentReply).HasForeignKey<UserBlogCommentReply>(UBCR => UBCR.BlogCommentReplyId).OnDelete(DeleteBehavior.NoAction); ;
            builder.Property(B => B.ReplyDate).HasDefaultValueSql("GetUtcDate()");

            builder.HasKey(UBCR => UBCR.BlogCommentReplyId);
            builder.Property(UBCR => UBCR.BlogCommentReplyId).ValueGeneratedNever();
        }
    }
}
