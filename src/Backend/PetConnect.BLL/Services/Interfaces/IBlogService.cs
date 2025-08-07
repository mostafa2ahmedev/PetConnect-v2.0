using PetConnect.BLL.Services.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IBlogService
    {  
        IEnumerable<ReadBlogDataDto> GetAllReadBlogs();
        
        IEnumerable<ReadWriteBlogDataDto> GetAllReadWriteBlogs();

        IEnumerable<CommentDataDto> GetAllCommentsForSpecificBlog(Guid BlogId);

        IEnumerable<ReplyDataDto> GetAllRepliesForSpecificComment(Guid CommentId);

        Task<bool> AddBlog(string DoctorId ,AddBlogDto AddedBlogDto);
        Task<bool> AddBlogComment(string UserId, AddCommentDto AddedBlogDto);
        Task<bool> AddBlogCommentReply(string UserId, AddReplyDto AddedBlogDto);

        string? ToggleBlogLike(string UserId,Guid BlogId);
        string? ToggleCommentLike(string UserId, Guid CommentId);
        string? ToggleReplyLike(string UserId, Guid ReplyId);
        bool UpdateBlog(UpdateBlogDto AddedBlogDto);

        bool DeleteBlog(Guid BlogId);




    }
}
