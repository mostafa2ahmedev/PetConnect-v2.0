using Microsoft.EntityFrameworkCore;
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
    public class UserBlogCommentReplyRepository : GenericRepository<UserBlogCommentReply>,IUserBlogCommentReplyRepository
    {
        private readonly AppDbContext context;

        public UserBlogCommentReplyRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }
        public IQueryable<UserBlogCommentReply> GetAllRepliesByCommentId(Guid CommentId)
        {

            return context.UserBlogCommentReplies.Include(UBCR=>UBCR.BlogCommentReply).Where(UBCR => UBCR.BlogCommentId == CommentId); ;
        }
        public int GetNumberOfRepliesByCommentId(Guid CommentId)
        {
            return context.UserBlogCommentReplies.Where(UBCR=>UBCR.BlogCommentId==CommentId).Count();
        }
    }
}
