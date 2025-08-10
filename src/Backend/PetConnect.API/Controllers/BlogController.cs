using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Blog;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }




        [HttpGet("AllBlogs")]
        [ProducesResponseType(typeof(List<BlogData>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Blogs Blog Data")]
        public ActionResult GetAllBlogs([FromQuery] int? Topic = null, [FromQuery] int? CategoryId = null)
        {
            var Blogs = _blogService.GetAllBlogs(Topic, CategoryId);
            return Ok(new GeneralResponse(200, Blogs));
        }

        [HttpGet("Blog/{BlogId}")]
        [ProducesResponseType(typeof(List<BlogDetails>), StatusCodes.Status200OK)]
        [EndpointSummary("Get Blog Details by ID")]
        public ActionResult GetBlogDetails(Guid BlogId)
        {
            var UserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            var Blog = _blogService.GetBlogById(BlogId, UserId);
            if(Blog == null)
                return NotFound(new GeneralResponse(404,$"Blog with ID {BlogId} Not Found"));
            return Ok(new GeneralResponse(200, Blog));
        }



        [HttpGet("ReadWriteBlogs/Comments/{BlogId}")]
        [ProducesResponseType(typeof(List<CommentDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All {{{{{Comments}}}}} For Blogs")]
        public ActionResult GetAllCommentsForReadWriteBlogs(Guid BlogId)
        {
            var UserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            var Comments = _blogService.GetAllCommentsForSpecificBlog(BlogId, UserId);
            return Ok(new GeneralResponse(200, Comments));
        }



        [HttpGet("Replies/{CommentId}")]
        [ProducesResponseType(typeof(List<ReplyDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All {{{{{Replies}}}}} For Specific Comment")]
        public ActionResult GetAllRepliesForComment(Guid CommentId)
        {
            var UserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var Replies = _blogService.GetAllRepliesForSpecificComment(CommentId, UserId);
            return Ok(new GeneralResponse(200, Replies));
        }




        [HttpPost("NewBlog")]
        [EndpointSummary("Add a new Blog")]
        [Authorize(Roles = "Doctor")]

        public async Task<ActionResult> AddBlog([FromForm]AddBlogDto addBlogDto)
        {
            if (!ModelState.IsValid )
            {
                return BadRequest(new GeneralResponse(400, "Data is not correct"));
           
            }
            else {
                var DoctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _blogService.AddBlog(DoctorId!, addBlogDto);

                if(result)
                    return Ok(new GeneralResponse(200, "Data Saved Successfully"));
                else
                    return BadRequest(new GeneralResponse(400, "ErrorHappened"));
            }

        }


        [HttpPost("NewComment")]
        [EndpointSummary("Add a new Comment")]
        [Authorize()]

        public async Task<ActionResult> AddComment([FromForm] AddCommentDto addCommentDto)
        {
            if ((addCommentDto.Comment == null && addCommentDto.Media == null) || !ModelState.IsValid)
            {
                return BadRequest(new GeneralResponse(400, "Data is not correct"));

            }
            else
            {
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _blogService.AddBlogComment(UserId!, addCommentDto);

                if (result)
                    return Ok(new GeneralResponse(200, "Data Saved Successfully"));
                else
                    return BadRequest(new GeneralResponse(400, "ErrorHappened"));
            }

        }
        [HttpPost("NewReply")]
        [EndpointSummary("Add a new Reply On Comment")]
        [Authorize()]

        public async Task<ActionResult> AddReply([FromForm] AddReplyDto addReplyDto)
        {
            if ((addReplyDto.Reply == null && addReplyDto.Media == null) || !ModelState.IsValid)
            {
                return BadRequest(new GeneralResponse(400, "Data is not correct"));

            }
            else
            {
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _blogService.AddBlogCommentReply(UserId!, addReplyDto);

                if (result)
                    return Ok(new GeneralResponse(200, "Data Saved Successfully"));
                else
                    return BadRequest(new GeneralResponse(400, "ErrorHappened"));
            }

        }

        [HttpPost("BlogLike/{BlogId}")]
        [EndpointSummary("Toggle Blog Like")]
        [Authorize()]
        public  ActionResult ToggleBlogLike(Guid BlogId)
        {
     
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result =  _blogService.ToggleBlogLike(UserId!, BlogId);
            if (result == null)
                return NotFound(new GeneralResponse(400, "Not Found"));
             else
                return Ok(new GeneralResponse(200, result));

        }



        [HttpPost("CommentLike/{CommentId}")]
        [EndpointSummary("Toggle Comment Like")]
        [Authorize()]
        public ActionResult ToggleCommentLike(Guid CommentId)
        {

            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _blogService.ToggleCommentLike(UserId!, CommentId);
            if (result == null)
                return NotFound(new GeneralResponse(400, "Not Found"));
            else
                return Ok(new GeneralResponse(200, result));

        }


        [HttpPost("ReplyLike/{ReplyId}")]
        [EndpointSummary("Toggle Reply Like")]
        [Authorize()]
        public ActionResult ToggleReplyLike(Guid ReplyId)
        {

            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _blogService.ToggleReplyLike(UserId!, ReplyId);
            if (result == null)
                return NotFound(new GeneralResponse(400, "Not Found"));
            else
                return Ok(new GeneralResponse(200, result));

        }
        [HttpPut("EditBlog")]
        [EndpointSummary("Update Blog Data")]
        [Authorize()]
        public async Task<ActionResult> EditBlog([FromForm] UpdateBlogDto updateBlogDto)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0) 
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new GeneralResponse(400, errors));
            }

            var result =await _blogService.UpdateBlog(updateBlogDto);
            if (!result)
                return NotFound(new GeneralResponse(400, "Not Found"));
            else
                return Ok(new GeneralResponse(200, "Blog Updated Successfully"));

        }
        [HttpPut("EditComment")]
        [EndpointSummary("Update Comment Data")]
        [Authorize()]
        public async Task<ActionResult> EditComment([FromForm] UpdateCommentDto updateCommentDto)
        {

            if ((updateCommentDto.Comment == null && updateCommentDto.Media == null) || !ModelState.IsValid)
            {
              

                return BadRequest(new GeneralResponse(400, "You Should Provide Media Or Content"));
            }

            var result = await _blogService.UpdateComment(updateCommentDto);
            if (!result)
                return NotFound(new GeneralResponse(400, "Not Found"));
            else
                return Ok(new GeneralResponse(200, "Comment Updated Successfully"));

        }
        [HttpPut("EditReply")]
        [EndpointSummary("Update Reply Data")]
        [Authorize()]
        public async Task<ActionResult> EditReply([FromForm] UpdateReplyDto updateReplyDto)
        {

            if ((updateReplyDto.Reply == null && updateReplyDto.Media == null) || !ModelState.IsValid)
            {


                return BadRequest(new GeneralResponse(400, "You Should Provide Media Or Content"));
            }

            var result = await _blogService.UpdateReply(updateReplyDto);
            if (!result)
                return NotFound(new GeneralResponse(400, "Not Found"));
            else
                return Ok(new GeneralResponse(200, "Reply Updated Successfully"));

        }
        [HttpDelete("DeleteBlog/{BLogId}")]
        [EndpointSummary("Delete Blog")]
        [Authorize()]
        public  ActionResult DeleteBlog(Guid? BLogId)
        {

          if(BLogId == null)
                return BadRequest(new GeneralResponse(400, "Blog ID Can't be null"));

            var result =  _blogService.DeleteBlog(BLogId.Value);
            if (!result)
                return NotFound(new GeneralResponse(400, "Not Found"));
            else
                return Ok(new GeneralResponse(200, "Blog Deleted Successfully"));

        }
        [HttpDelete("DeleteComment/{CommentId}")]
        [EndpointSummary("Delete Comment")]
        [Authorize()]
        public ActionResult DeleteComment(Guid? CommentId)
        {

            if (CommentId == null)
                return BadRequest(new GeneralResponse(400, "Comment ID Can't be null"));

            var result = _blogService.DeleteComment(CommentId.Value);
            if (!result)
                return NotFound(new GeneralResponse(400, "Not Found"));
            else
                return Ok(new GeneralResponse(200, "Comment Deleted Successfully"));

        }
        [HttpDelete("DeleteReply/{ReplyId}")]
        [EndpointSummary("Delete Reply")]
        [Authorize()]
        public ActionResult DeleteReply(Guid? ReplyId)
        {

            if (ReplyId == null)
                return BadRequest(new GeneralResponse(400, "Reply ID Can't be null"));

            var result = _blogService.DeleteReply(ReplyId.Value);
            if (!result)
                return NotFound(new GeneralResponse(400, "Not Found"));
            else
                return Ok(new GeneralResponse(200, "Reply Deleted Successfully"));

        }
    }
}
