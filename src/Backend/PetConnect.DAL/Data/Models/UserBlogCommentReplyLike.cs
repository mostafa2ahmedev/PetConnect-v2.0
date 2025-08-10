using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class UserBlogCommentReplyLike
    {
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public Guid BlogCommentReplyId { get; set; }
        public BlogCommentReply BlogCommentReply { get; set; } = null!;

        public DateTime Date { get; set; }

        public CommentORReplyORLikeType UserType { get; set; }
    }
}
