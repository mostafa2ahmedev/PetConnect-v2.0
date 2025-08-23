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
    public class BlogCommentConfigurations : IEntityTypeConfiguration<BlogComment>
    {
        public void Configure(EntityTypeBuilder<BlogComment> builder)
        {
            builder.HasKey(B => B.ID);
            builder.Property(B => B.ID).ValueGeneratedNever();
            builder.Property(B => B.Comment).HasColumnType("varchar(max)");
            builder.Property(B => B.Media).HasColumnType("varchar(200)");

            builder.Property(B => B.CommentORReplyType)
            .HasConversion(
            CommentORReplyType => CommentORReplyType.ToString(),
            returnCommentORReplyType => (CommentORReplyORLikeType)Enum.Parse(typeof(CommentORReplyORLikeType), returnCommentORReplyType)
            );
        }
    }
}
