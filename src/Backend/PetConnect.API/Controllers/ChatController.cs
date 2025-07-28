using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Chat;
using PetConnect.BLL.Services.Interfaces;
using System.Security.Claims;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }


        [HttpGet("Load/{ReceiverId}")]
        [Authorize(Roles ="Customer,Admin")]
        [ProducesResponseType(typeof(List<UserMessageDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Chat Messages between the RequesterId(From Token) and ReceiverId (Provided from you ya KING)")]

        public ActionResult LoadAllChat(string ReceiverId) {

            if (ReceiverId == null)
                return BadRequest();
            var senderId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _chatService.LoadChat(senderId!, ReceiverId, 1);

            return Ok(result);
        }


        [HttpGet("Messenger")]
        [Authorize(Roles = "Customer,Admin")]
        [ProducesResponseType(typeof(List<UserBannerDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Users you contact with before (like Messenger) ya KING")]
        public ActionResult GetMessenger()
        {

            var senderId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _chatService.LoadUserMessengersBySenderId(senderId!);

            return Ok(result);
        }

   
    }
}
