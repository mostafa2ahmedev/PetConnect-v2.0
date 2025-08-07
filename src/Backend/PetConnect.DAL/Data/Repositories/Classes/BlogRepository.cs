using Microsoft.EntityFrameworkCore;
using PetConnect.DAL.Data.Enums;
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
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable<Blog> GetAllReadBlogs() { 
        
        return _context.Blogs.Include(B=>B.Doctor).Include(B=>B.UserBlogComments).Include(B=>B.UserBlogLikes).Where(B => B.BlogType == BlogType.Read);
        }
        public IQueryable<Blog> GetAllReadWriteBlogs()
        {

            return _context.Blogs.Include(B => B.Doctor).Include(B => B.UserBlogComments).Include(B => B.UserBlogLikes).Where(B => B.BlogType == BlogType.ReadWrite);
        }

    }
}
