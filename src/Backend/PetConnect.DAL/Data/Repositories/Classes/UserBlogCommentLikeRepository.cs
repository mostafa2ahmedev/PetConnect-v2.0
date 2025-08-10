using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    public class UserBlogCommentLikeRepository : GenericRepository<UserBlogCommentLike>, IUserBlogCommentLikeRepository
    {
        private readonly AppDbContext context;

        public UserBlogCommentLikeRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public int GetNumberOfLikesForSpecificComment(Guid CommentId)
        {

            return context.UserBlogCommentLikes.Where(UBCL => UBCL.BlogCommentId == CommentId).Count();
        }

        public UserBlogCommentLike? GetUserBlogCommentLike(string UserId, Guid CommentId)
        {

            return context.UserBlogCommentLikes.Where(UBCL => UBCL.UserId == UserId && UBCL.BlogCommentId == CommentId).FirstOrDefault();
        }

        public bool IsCommentLikedByUser(Guid commentId, string userId)
        {
            return context.UserBlogCommentLikes
                .Any(like => like.BlogCommentId == commentId && like.UserId == userId);
        }
    }
}
