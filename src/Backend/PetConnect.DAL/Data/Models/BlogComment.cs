using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class BlogComment
    {
        public Guid ID { get; set; }
        public string? Comment { get; set; } = null!;
        public string? Media { get; set; } = null!;

        public UserBlogComment UserBlogComment { get; set; } = null!;
        public ICollection<UserBlogCommentReply> UserBlogCommentReplies { get; set; } = new HashSet<UserBlogCommentReply>();

        public ICollection<UserBlogCommentLike> UserBlogCommentLikes { get; set; } = new HashSet<UserBlogCommentLike>();

        public CommentORReplyORLikeType CommentORReplyType { get; set; }

        public BlogComment()
        {
            ID = Guid.NewGuid();
        }

    }
}
