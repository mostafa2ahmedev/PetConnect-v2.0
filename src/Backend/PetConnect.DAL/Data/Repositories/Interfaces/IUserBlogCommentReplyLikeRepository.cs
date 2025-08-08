using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Interfaces
{
    public interface IUserBlogCommentReplyLikeRepository : IGenericRepository<UserBlogCommentReplyLike>
    {
        public int GetNumberOfLikesForSpecificReply(Guid ReplyId);
        public UserBlogCommentReplyLike? GetUserBlogCommentReplyLike(string UserId, Guid ReplyId);
        public bool IsCommentLikedByUser(Guid commentId, string userId);
    }
}
