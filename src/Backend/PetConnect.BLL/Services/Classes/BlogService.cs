using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTO.Account;
using PetConnect.BLL.Services.DTOs.Blog;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Classes;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;


        public BlogService(IUnitOfWork unitOfWork,IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
  
        }


        public IEnumerable<BlogData> GetAllBlogs(int? Topic, int? PetCategoryId)
        {
            return _unitOfWork.BlogRepository.GetAllBlogsWithAuthorDataAndSomeStatistics(Topic , PetCategoryId)
                  .Select(B => new BlogData()
                  {
                      ID = B.ID,
                      BlogType = B.BlogType,
                      excerpt = B.excerpt,
                      Title = B.Title,
                      Media = B.Media,
                      PostDate = B.PostDate,
                      DoctorId = B.DoctorId,
                      Likes = B.UserBlogLikes.Count,
                      DoctorName = B.Doctor.FName + " " + B.Doctor.LName,
                      DoctorImgUrl = B.Doctor.ImgUrl,
                      Comments = B.UserBlogComments.Count,
                      Topic = B.Topic.ToString(),
                      PetCategoryName = B.PetCategory.Name,




                  });
        }
        public BlogDetails? GetBlogById(Guid BlogId, string UserId)
        {
            var Blog = _unitOfWork.BlogRepository.GetBlogByIdWithAuthorDataAndSomeStatistics(BlogId);
            if (Blog == null)
                return null;
            return new BlogDetails() {
                ID = Blog.ID,
                BlogType = Blog.BlogType,
                excerpt = Blog.excerpt,
                Title = Blog.Title,
                Media = Blog.Media,
                Content = Blog.Content,
                PostDate = Blog.PostDate,
                DoctorId = Blog.DoctorId,
                Likes = Blog.UserBlogLikes.Count,
                DoctorName = Blog.Doctor.FName + " " + Blog.Doctor.LName,
                DoctorImgUrl = Blog.Doctor.ImgUrl,
                Comments = Blog.UserBlogComments.Count,
                IsLikedByUser = Blog.UserBlogLikes.Any(like => like.UserId == UserId), // ✅
                Topic = Blog.Topic.ToString(),
                PetCategoryName = Blog.PetCategory.Name,

            };
        }
        public IEnumerable<CommentDataDto> GetAllCommentsForSpecificBlog(Guid BlogId, string UserId)
        {
            return _unitOfWork.UserBlogCommentRepository.GetAllCommentsByBlogIdWithAuthorAndBlogData(BlogId)
                .Select(UBC => new CommentDataDto()
                {
                    IsDeleted = UBC.IsDeleted,
                    Comment = UBC.BlogComment.Comment,
                    ID = UBC.BlogCommentId,
                    Media = UBC.BlogComment.Media,
                    PosterImage = UBC.User.ImgUrl,
                    PosterName = UBC.User.FName + " " + UBC.User.LName,
                    NumberOfLikes = _unitOfWork.UserBlogCommentLikeRepository.GetNumberOfLikesForSpecificComment(UBC.BlogCommentId),
                    NumberOfReplies = _unitOfWork.UserBlogCommentReplyRepository.GetNumberOfRepliesByCommentId(UBC.BlogCommentId),

                    IsLikedByUser = _unitOfWork.UserBlogCommentLikeRepository
                .IsCommentLikedByUser(UBC.BlogCommentId, UserId)
                });
        }

    
        public IEnumerable<ReplyDataDto> GetAllRepliesForSpecificComment(Guid CommentId, string UserId)
        {
            return _unitOfWork.UserBlogCommentReplyRepository.GetAllRepliesByCommentId(CommentId)
                   .Select(UBCR => new ReplyDataDto()
                   {
                       IsDeleted = UBCR.IsDeleted,
                       Reply = UBCR.BlogCommentReply.CommentReply,
                       ID = UBCR.BlogCommentReplyId,
                       Media = UBCR.BlogCommentReply.Media,
                       PosterImage = UBCR.User.ImgUrl,
                       PosterName = UBCR.User.FName + " " + UBCR.User.LName,
                       NumberOfLikes = _unitOfWork.UserBlogCommentReplyLikeRepository.GetNumberOfLikesForSpecificReply(UBCR.BlogCommentReplyId),
                       IsLikedByUser = _unitOfWork.UserBlogCommentReplyLikeRepository
                  
                       
                .IsCommentLikedByUser(UBCR.BlogCommentReplyId, UserId)
                   });
        }




        public async Task<bool> AddBlog(string DoctorId ,AddBlogDto AddedBlogDto)
        {
            string? fileName=null;

            if (AddedBlogDto.Media != null)
            {
                fileName = await _attachmentService.UploadAsync(AddedBlogDto.Media, "img/blogs");
            }

            var Blog = new Blog()
            {
                BlogType = AddedBlogDto.BlogType,
                Content = AddedBlogDto.Content ?? string.Empty,
                DoctorId = DoctorId,
                IsApproved = false,
                Media = fileName != null ? $"/assets/img/blogs/{fileName}" : null,
                excerpt = AddedBlogDto.excerpt,
                Title = AddedBlogDto.Title,
                IsDeleted = false,
                Topic = AddedBlogDto.Topic,
                PetCategoryId = AddedBlogDto.PetCategoryId

            };
            
            _unitOfWork.BlogRepository.Add(Blog);
           var result= _unitOfWork.SaveChanges();

            return result >= 1? true:false;
        }


        public async Task<bool> AddBlogComment(string UserId, AddCommentDto AddCommentDto)
        {
            string? fileName = null;

            if (AddCommentDto.Media != null)
            {
                fileName = await _attachmentService.UploadAsync(AddCommentDto.Media, "img/blogs/comments");
            }


            var BlogComment = new BlogComment() {
            Comment = AddCommentDto.Comment?? string.Empty,
            Media = fileName != null ? $"/assets/img/blogs/comments/{fileName}" : string.Empty,
        

            };
            Console.WriteLine(BlogComment.ID);
            var UserBlogComment = new UserBlogComment() { 
            BlogCommentId = BlogComment.ID,
            BlogId = AddCommentDto.BlogId,
             UserId = UserId,
            IsDeleted = false,
            
            };
            _unitOfWork.BlogCommentRepository.Add(BlogComment);
            _unitOfWork.UserBlogCommentRepository.Add(UserBlogComment);
            var result=  _unitOfWork.SaveChanges();

           return result >=  1? true:false;
        }

        public async Task<bool> AddBlogCommentReply(string UserId, AddReplyDto AddReplyDto)
        {
            string? fileName = null;

            if (AddReplyDto.Media != null)
            {
                fileName = await _attachmentService.UploadAsync(AddReplyDto.Media, "img/blogs/comments/replies");
            }


            var BlogCommentReply = new BlogCommentReply()
            {
                CommentReply = AddReplyDto.Reply ?? string.Empty,
                Media = fileName != null ? $"/assets/img/blogs/comments/replies/{fileName}" : string.Empty,
              



            };
            var UserBlogCommentReply = new UserBlogCommentReply()
            {
                BlogCommentId = AddReplyDto.CommentId,
                BlogCommentReplyId = BlogCommentReply.ID,
                 UserId = UserId,
                 IsDeleted=false,
            };
            _unitOfWork.BlogCommentReplyRepository.Add(BlogCommentReply);
            _unitOfWork.UserBlogCommentReplyRepository.Add(UserBlogCommentReply);
            var result =  _unitOfWork.SaveChanges();

            return result >= 1 ? true : false;
        }

        public string? ToggleBlogLike(string UserId, Guid BlogId)
        {
            string? result;
            var UserBlogLike = _unitOfWork.UserBlogLikeRepository.GetUserBlogLikeRecord(UserId, BlogId);

            if (UserBlogLike is not { }) {
                _unitOfWork.UserBlogLikeRepository.Add(new UserBlogLike()
                {
                    UserId = UserId,
                    BlogId = BlogId
                });
                result = "On";
            }

            else {

                _unitOfWork.UserBlogLikeRepository.Delete(UserBlogLike);
                result = "Off";
            }
               

           _unitOfWork.SaveChanges();

            return result;
        }

        public string? ToggleCommentLike(string UserId,Guid CommentId)
        {
            string? result;
            var UserBlogCommentLike = _unitOfWork.UserBlogCommentLikeRepository.GetUserBlogCommentLike(UserId, CommentId);

            if (UserBlogCommentLike is not { })
            {
                _unitOfWork.UserBlogCommentLikeRepository.Add(new UserBlogCommentLike()
                {
                    UserId = UserId,
                    BlogCommentId = CommentId
                });
                result = "On";
            }

            else
            {

                _unitOfWork.UserBlogCommentLikeRepository.Delete(UserBlogCommentLike);
                result = "Off";
            }


            _unitOfWork.SaveChanges();

            return result;
        }

        public string? ToggleReplyLike(string UserId,Guid ReplyId)
        {
            string? result;
            var UserBlogCommentReplyLike = _unitOfWork.UserBlogCommentReplyLikeRepository.GetUserBlogCommentReplyLike(UserId, ReplyId);

            if (UserBlogCommentReplyLike is not { })
            {
                _unitOfWork.UserBlogCommentReplyLikeRepository.Add(new UserBlogCommentReplyLike()
                {
                    UserId = UserId,
                    BlogCommentReplyId = ReplyId
                });
                result = "On";
            }

            else
            {

                _unitOfWork.UserBlogCommentReplyLikeRepository.Delete(UserBlogCommentReplyLike);
                result = "Off";
            }


            _unitOfWork.SaveChanges();

            return result;
        }

        public async Task<bool> UpdateBlog(UpdateBlogDto UpdateBlogDto)
        {
            var Blog = _unitOfWork.BlogRepository.GetByID(UpdateBlogDto.BlogId);
            if (Blog == null)
                return false;

            if (UpdateBlogDto.Media != null)
            {
                var fileName = await _attachmentService.UploadAsync(UpdateBlogDto.Media, Path.Combine("img", "blogs"));
                if (!string.IsNullOrEmpty(fileName))
                {
                    Blog.Media = $"/assets/img/blogs/{fileName}";
                }
            }

            Blog.Title = UpdateBlogDto.Title;
            Blog.excerpt = UpdateBlogDto.excerpt;
            Blog.Content = UpdateBlogDto.Content;
            Blog.BlogType = UpdateBlogDto.BlogType;
            Blog.Topic = UpdateBlogDto.Topic;
            Blog.PetCategoryId = UpdateBlogDto.PetCategoryId;




            _unitOfWork.BlogRepository.Update(Blog);
            return  await _unitOfWork.SaveChangesAsync()>=1?true:false;
        }

        public async Task<bool> UpdateComment(UpdateCommentDto UpdateCommentDto)
        {
            var CommentRecord = _unitOfWork.BlogCommentRepository.GetByID(UpdateCommentDto.CommentId);
            if (CommentRecord == null)
                return false;

            if (UpdateCommentDto.Media != null)
            {
                var fileName = await _attachmentService.UploadAsync(UpdateCommentDto.Media, Path.Combine("img", "blogs", "comments"));
                if (!string.IsNullOrEmpty(fileName))
                {
                    CommentRecord.Media = $"/assets/img/blogs/comments/{fileName}";
                }
            }

          CommentRecord.Comment = UpdateCommentDto.Comment;   
         




            _unitOfWork.BlogCommentRepository.Update(CommentRecord);
            return await _unitOfWork.SaveChangesAsync() >= 1 ? true : false;
        }

        public async Task<bool> UpdateReply(UpdateReplyDto UpdateReplyDto)
        {
            var ReplyRecord = _unitOfWork.BlogCommentReplyRepository.GetByID(UpdateReplyDto.ReplyId);
            if (ReplyRecord == null)
                return false;

            if (UpdateReplyDto.Media != null)
            {
                var fileName = await _attachmentService.UploadAsync(UpdateReplyDto.Media, Path.Combine("img", "blogs", "comments", "replies"));
                if (!string.IsNullOrEmpty(fileName))
                {
                    ReplyRecord.Media = $"/assets/img/blogs/comments/replies/{fileName}";
                }
            }

            ReplyRecord.CommentReply = UpdateReplyDto.Reply;





            _unitOfWork.BlogCommentReplyRepository.Update(ReplyRecord);
            return await _unitOfWork.SaveChangesAsync() >= 1 ? true : false;
        }
        public bool DeleteBlog(Guid BlogId)
        {
            bool result = false;
            var Blog = _unitOfWork.BlogRepository.GetByID(BlogId);

            if (Blog == null)
                return false;

            var UserBlogComments = _unitOfWork.UserBlogCommentRepository.GetAllUserCommentsByBlogId(BlogId);

            foreach (var Comment in UserBlogComments)
            {
                Comment.IsDeleted = true;
                _unitOfWork.UserBlogCommentRepository.Update(Comment);
                var UserBlogCommentReplies = _unitOfWork.UserBlogCommentReplyRepository.GetAllUserCommentRepliesByCommentId(Comment.BlogCommentId);
                foreach (var Reply in UserBlogCommentReplies)
                {
                    Reply.IsDeleted = true;
                    _unitOfWork.UserBlogCommentReplyRepository.Update(Reply);
                }

            }

            Blog.IsDeleted = true;
            _unitOfWork.BlogRepository.Update(Blog);
            result = _unitOfWork.SaveChanges() >= 1 ? true : false;

            return result;
        }
        public bool DeleteComment(Guid CommentId)
        {
            var Comment = _unitOfWork.UserBlogCommentRepository.GetByID(CommentId);
            bool result = false;
            if (Comment == null)
                return false;

            Comment.IsDeleted= true;
            _unitOfWork.UserBlogCommentRepository.Update(Comment);
            result = _unitOfWork.SaveChanges() >= 1 ? true : false;

            return result;
        }

        public bool DeleteReply(Guid ReplyId)
        {
            var Reply = _unitOfWork.UserBlogCommentReplyRepository.GetByID(ReplyId);
            bool result = false;
            if (Reply == null)
                return false;

            Reply.IsDeleted = true;
            _unitOfWork.UserBlogCommentReplyRepository.Update(Reply);
            result = _unitOfWork.SaveChanges() >= 1 ? true : false;

            return result;
        }

    
    }
}
