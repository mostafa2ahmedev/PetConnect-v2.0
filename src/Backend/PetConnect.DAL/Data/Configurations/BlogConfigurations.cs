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
    public class BlogConfigurations : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
        
            builder.HasKey(B => B.ID);
            builder.Property(B => B.ID).ValueGeneratedNever();
            builder.Property(b => b.PostDate).HasDefaultValueSql("GetUtcDate()");
            builder.Property(B => B.Content).HasColumnType("varchar(max)");
            builder.Property(B => B.Title).HasColumnType("varchar(200)");
            builder.Property(B => B.excerpt).HasColumnType("varchar(500)");
            builder.Property(B => B.Media).HasColumnType("varchar(200)");
            builder.Property(B => B.BlogType)
             .HasConversion(
             BlogType => BlogType.ToString(),
             returnBlogType => (BlogType)Enum.Parse(typeof(BlogType), returnBlogType)
             );
            builder.Property(B => B.Topic)
             .HasConversion(
            BlogTopic => BlogTopic.ToString(),
             returnBlogTopic => (BlogTopic)Enum.Parse(typeof(BlogTopic), returnBlogTopic)
             );
           builder.HasOne(B => B.Doctor).WithMany(D => D.DoctorBlogs).HasForeignKey(B => B.DoctorId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(B => B.PetCategory).WithMany(PC => PC.Blogs).HasForeignKey(B => B.PetCategoryId);
        }
    }
}
