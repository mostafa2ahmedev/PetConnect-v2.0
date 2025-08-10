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
    public class UserBlogLikeRepository : GenericRepository<UserBlogLike>, IUserBlogLikeRepository
    {
        private readonly AppDbContext context;

        public UserBlogLikeRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public UserBlogLike? GetUserBlogLikeRecord(string UserId, Guid BlogId) { 
        
        return context.UserBlogLikes.Where(UBL=>UBL.UserId==UserId&& UBL.BlogId==BlogId).FirstOrDefault();  
        }
    }
}
