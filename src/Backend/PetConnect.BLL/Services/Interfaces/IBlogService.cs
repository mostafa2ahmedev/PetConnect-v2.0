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
        
        IEnumerable<BlogData> GetAllBlogs();
        BlogDetails? GetBlogById(Guid BlogId, string UserId);
        public IEnumerable<CommentDataDto> GetAllCommentsForSpecificBlog(Guid BlogId, string UserId);

        IEnumerable<ReplyDataDto> GetAllRepliesForSpecificComment(Guid CommentId, string UserId);

        Task<bool> AddBlog(string DoctorId ,AddBlogDto AddedBlogDto);
        Task<bool> AddBlogComment(string UserId, AddCommentDto AddedBlogDto);
        Task<bool> AddBlogCommentReply(string UserId, AddReplyDto AddedBlogDto);

        string? ToggleBlogLike(string UserId,Guid BlogId);
        string? ToggleCommentLike(string UserId, Guid CommentId);
        string? ToggleReplyLike(string UserId, Guid ReplyId);
        Task<bool> UpdateBlog(UpdateBlogDto UpdateBlogDto);
        Task<bool> UpdateComment(UpdateCommentDto UpdateCommentDto);
        Task<bool> UpdateReply(UpdateReplyDto UpdateReplyDto);
        bool DeleteBlog(Guid BlogId);
        bool DeleteComment(Guid CommentId);
        bool DeleteReply(Guid ReplyId);


    }
}
