using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Blog;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using System.Security.Claims;

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



        [HttpGet("ReadBlogs")]
        [ProducesResponseType(typeof(List<ReadBlogDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All {{{{{Read}}}}} Blog Data")]
        public ActionResult GetAllReadBlogData()
        {
            var ReadBlogs = _blogService.GetAllReadBlogs();
            return Ok(new GeneralResponse(200, ReadBlogs));
        }



        [HttpGet("ReadWriteBlogs")]
        [ProducesResponseType(typeof(List<ReadWriteBlogDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All {{{{{ReadWrite}}}}} Blog Data")]
        public ActionResult GetAllReadWriteBlogData()
        {
            var ReadWriteBlogs = _blogService.GetAllReadWriteBlogs();
            return Ok(new GeneralResponse(200, ReadWriteBlogs));
        }




        [HttpGet("/ReadWriteBlogs/Comments/{BlogId}")]
        [ProducesResponseType(typeof(List<CommentDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All {{{{{Comments}}}}} For ReadWrite Blogs")]
        public ActionResult GetAllCommentsForReadWriteBlogs(Guid BlogId)
        {
            var Comments = _blogService.GetAllCommentsForSpecificBlog(BlogId);
            return Ok(new GeneralResponse(200, Comments));
        }



        [HttpGet("/Replies/{CommentId}")]
        [ProducesResponseType(typeof(List<ReplyDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All {{{{{Replies}}}}} For Specific Comment")]
        public ActionResult GetAllRepliesForComment(Guid CommentId)
        {
            var Replies = _blogService.GetAllRepliesForSpecificComment(CommentId);
            return Ok(new GeneralResponse(200, Replies));
        }




        [HttpPost("NewBlog")]
        [EndpointSummary("Add a new Blog")]
        [Authorize(Roles = "Doctor")]

        public async Task<ActionResult> AddBlog([FromForm]AddBlogDto addBlogDto)
        {
            if ((addBlogDto.Content == null && addBlogDto.Media == null) || !ModelState.IsValid )
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
    }
}
