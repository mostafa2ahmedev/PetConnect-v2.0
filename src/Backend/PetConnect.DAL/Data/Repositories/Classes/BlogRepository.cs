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



        public IQueryable<Blog> GetAllBlogsWithAuthorDataAndSomeStatistics(int? Topic, int? PetCategoryId)
        {
            var query = _context.Blogs
                .Include(B => B.Doctor)
                .Include(B => B.UserBlogComments)
                .Include(B => B.UserBlogLikes)
                .Where(B => !B.IsDeleted);

        
            if (Topic.HasValue)
            {
                query = query.Where(B => B.Topic == (BlogTopic)Topic.Value);
            }

            
            if (PetCategoryId.HasValue)
            {
                query = query.Include(B=>B.PetCategory).Where(B => B.PetCategoryId==PetCategoryId);
            }

            return query;
        }
        public IQueryable<Blog> GetAllBlogsWithAuthorDataAndSomeStatisticsByDoctorId(string DoctorId)
        {
            return _context.Blogs.Include(B => B.Doctor).Include(B => B.UserBlogComments).Include(B => B.UserBlogLikes).Where(B => B.IsDeleted == false && B.DoctorId ==DoctorId);
        }

        public Blog? GetBlogByIdWithAuthorDataAndSomeStatistics(Guid BlogId)
        {
            return _context.Blogs.Include(B => B.Doctor).Include(B => B.UserBlogComments).Include(B => B.UserBlogLikes).FirstOrDefault(B=>B.ID==BlogId);
        }
    }
}
