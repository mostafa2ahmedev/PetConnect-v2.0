using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Interfaces
{
    public interface IUserBlogCommentReplyRepository : IGenericRepository<UserBlogCommentReply>
    {
        public IQueryable<UserBlogCommentReply> GetAllRepliesByCommentId(Guid CommentId);
        public int GetNumberOfRepliesByCommentId(Guid CommentId);
    }
}
