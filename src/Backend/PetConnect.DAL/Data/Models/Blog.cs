using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class Blog
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = null!; 
        public string excerpt { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Media { get; set; } = null!;
        public bool IsApproved { get; set; }

        public BlogType BlogType { get; set; }
        public DateTime PostDate { get; set; }

        public string DoctorId { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;



        public ICollection<UserBlogComment> UserBlogComments { get; set; } = new HashSet<UserBlogComment>();

        public ICollection<UserBlogLike> UserBlogLikes { get; set; } = new HashSet<UserBlogLike>();

        public Blog()
        {
            ID= Guid.NewGuid();
        }
    }
}
