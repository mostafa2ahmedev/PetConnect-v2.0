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
    public class BlogCommentReplyConfigurations : IEntityTypeConfiguration<BlogCommentReply>
    {
        public void Configure(EntityTypeBuilder<BlogCommentReply> builder)
        {
            builder.HasKey(BCR => BCR.ID);
            builder.Property(BCR => BCR.ID).ValueGeneratedNever();
            builder.Property(BCR => BCR.CommentReply).HasColumnType("varchar(max)");
            builder.Property(BCR => BCR.Media).HasColumnType("varchar(200)");

            builder.Property(BCR => BCR.CommentORReplyORLikeType)
              .HasConversion(
              CommentORReplyORLikeType => CommentORReplyORLikeType.ToString(),
              returnCommentORReplyORLikeType => (CommentORReplyORLikeType)Enum.Parse(typeof(CommentORReplyORLikeType), returnCommentORReplyORLikeType)
              );
        }
    }
}
