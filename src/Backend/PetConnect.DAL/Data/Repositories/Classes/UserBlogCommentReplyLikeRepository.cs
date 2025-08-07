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
    public class UserBlogCommentReplyLikeRepository : GenericRepository<UserBlogCommentReplyLike>, IUserBlogCommentReplyLikeRepository
    {
        private readonly AppDbContext context;

        public UserBlogCommentReplyLikeRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }
        public int GetNumberOfLikesForSpecificReply(Guid ReplyId)
        {

            return context.UserBlogCommentReplyLikes.Where(UBCRL => UBCRL.BlogCommentReplyId == ReplyId).Count();
        }

        public UserBlogCommentReplyLike? GetUserBlogCommentReplyLike(string UserId, Guid ReplyId)
        {

            return context.UserBlogCommentReplyLikes.Where(UBCRL => UBCRL.UserId == UserId && UBCRL.BlogCommentReplyId == ReplyId).FirstOrDefault();
        }

    }
}
