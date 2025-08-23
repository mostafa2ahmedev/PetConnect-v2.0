using Microsoft.EntityFrameworkCore;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    internal class UserBlogCommentRepository : GenericRepository<UserBlogComment>, IUserBlogCommentRepository
    {
        private readonly AppDbContext context;

        public UserBlogCommentRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }
        public IQueryable<UserBlogComment> GetAllCommentsByBlogIdWithAuthorAndBlogData(Guid BlogId)
        {

            return context.UserBlogComments.Include(UBC=>UBC.BlogComment).Include(UBC => UBC.User).Where(UBC => UBC.BlogId == BlogId && UBC.IsDeleted == false);
        }
        public IEnumerable<UserBlogComment> GetAllUserCommentsByBlogId(Guid BlogId)
        {
            return context.UserBlogComments.Where(UBCR => UBCR.BlogId == BlogId && UBCR.IsDeleted);
        }
    }
}
