
using Microsoft.AspNetCore.Identity;

using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;

namespace PetConnect.DAL.Data.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public bool IsApproved { get; set; } 
        public Gender Gender{ get; set; }
        public Address Address { get; set; } = null!;
        public string? ImgUrl{ get; set; } = null!;
        public bool IsDeleted { get; set; }
        public ICollection<UsersMessages> SentMessages { get; set; } = new HashSet<UsersMessages>();
        public ICollection<UsersMessages> ReceivedMessages { get; set; } = new HashSet<UsersMessages>();
        public ICollection<UserConnection> UserConnections { get; set; } = new HashSet<UserConnection>();
        public ICollection<Notification> Notifications{ get; set; } = new HashSet<Notification>();

        public ICollection<UserBlogComment> UserBlogComments { get; set; } = new HashSet<UserBlogComment>();

        public ICollection<UserBlogCommentReply> UserBlogCommentReplies { get; set; } = new HashSet<UserBlogCommentReply>();

        public ICollection<UserBlogLike> UserBlogLikes { get; set; } = new HashSet<UserBlogLike>();
        public ICollection<UserBlogCommentLike> UserBlogCommentLikes { get; set; } = new HashSet<UserBlogCommentLike>();
        public ICollection<UserBlogCommentReplyLike> UserBlogCommentReplyLikes { get; set; } = new HashSet<UserBlogCommentReplyLike>();
        public ICollection<SupportRequest> SupportRequests { get; set; } = new HashSet<SupportRequest>();
    }
}
