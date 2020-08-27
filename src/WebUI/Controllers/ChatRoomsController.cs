using System.Threading.Tasks;
using Application.ChatRooms.Commands.CreateChatRoom;
using Application.ChatRooms.Queries.GetChatRoom;
using Application.ChatRooms.Queries.GetMessagesOfRoom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.ChatRooms.Queries.GetChatRoomsOfUser;
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;
using rentasgt.Application.Common.Models;
using rentasgt.WebUI.Controllers;

namespace WebUI.Controllers
{

    [Authorize]
    public class ChatRoomsController : ApiController
    {
        
        [HttpGet]
        public async Task<ActionResult<PaginatedListResponse<ChatRoomDto>>> Get(
            int pageSize = 25, int pageNumber = 1) {
            return await Mediator.Send(new GetChatRoomsOfUserQuery {
                PageSize = pageSize,
                PageNumber = pageNumber,
            });
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<ChatRoomDto>> GetRoomForProduct(long productId)
        {
            return await Mediator.Send(new GetChatRoomQuery {
                ProductId = productId
            });
        }

        [HttpGet("{id}/messages")]
        public async Task<ActionResult<PaginatedListResponse<ChatMessageDto>>> GetMessages(
            long id,
            int pageSize = 35, int pageNumber = 1
        ) {
            return await Mediator.Send(new GetMessagesOfRoomQuery {
                PageSize = pageSize,
                PageNumber = pageNumber,
                ChatRoomId = id
            });
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateChatRoomCommand command)
        {
            return await Mediator.Send(command);
        }

    }
}