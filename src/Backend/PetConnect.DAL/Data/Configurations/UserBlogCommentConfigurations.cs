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
    public class UserBlogCommentConfigurations : IEntityTypeConfiguration<UserBlogComment>
    {
        public void Configure(EntityTypeBuilder<UserBlogComment> builder)
        {
            builder.HasOne(UBC => UBC.BlogComment).WithOne(BC => BC.UserBlogComment).HasForeignKey<UserBlogComment>(UBC => UBC.BlogCommentId).OnDelete(DeleteBehavior.NoAction); ;
            builder.HasOne(UBC => UBC.User).WithMany(U=> U.UserBlogComments).HasForeignKey(UBC => UBC.UserId).OnDelete(DeleteBehavior.NoAction); ;
            builder.HasOne(UBC => UBC.Blog).WithMany(B => B.UserBlogComments).HasForeignKey(UBC => UBC.BlogId).OnDelete(DeleteBehavior.NoAction); ;
            builder.Property(B => B.CommentDate).HasDefaultValueSql("GetUtcDate()");

            builder.HasKey(UBC => UBC.BlogCommentId);
            builder.Property(UBC => UBC.BlogCommentId).ValueGeneratedNever();



        }
    }
}
