using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        IQueryable<Blog> GetAllBlogsWithAuthorDataAndSomeStatistics(int? Topic, int? PetCategoryId);
        IQueryable<Blog> GetAllBlogsWithAuthorDataAndSomeStatisticsByDoctorId(string DoctorId);
        Blog? GetBlogByIdWithAuthorDataAndSomeStatistics(Guid BlogId);
       
    }
}
