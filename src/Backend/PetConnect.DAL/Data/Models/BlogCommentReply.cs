using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class BlogCommentReply
    {
        public Guid ID { get; set; }
        public string CommentReply { get; set; } = null!;
        public string Media { get; set; } = null!;

        public UserBlogCommentReply UserBlogCommentReply { get; set; } = null!;
        public ICollection<UserBlogCommentReplyLike> UserBlogCommentReplyLikes { get; set; } = new HashSet<UserBlogCommentReplyLike>();

        public CommentORReplyORLikeType CommentORReplyORLikeType { get; set; }
        public BlogCommentReply()
        {
            ID = Guid.NewGuid();
        }
    }
}
