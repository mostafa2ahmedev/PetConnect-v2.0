using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class UserBlogComment
    {
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; } = null!;
        public Guid BlogCommentId { get; set; }
        public BlogComment BlogComment { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public DateTime CommentDate { get; set; }
        public CommentORReplyORLikeType UserType { get; set; }
    }
}
