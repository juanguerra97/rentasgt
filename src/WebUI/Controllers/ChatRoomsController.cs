using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.ChatRooms.Queries.GetChatRoomsOfUser;
using rentasgt.Application.Common.Models;
using rentasgt.WebUI.Controllers;

namespace WebUI.Controllers
{

    [Authorize]
    public class ChatRoomsController : ApiController
    {
        
        public async Task<ActionResult<PaginatedListResponse<ChatRoomDto>>> Get(
            int pageSize = 15, int pageNumber = 1) {
            return await Mediator.Send(new GetChatRoomsOfUserQuery {
                PageSize = pageSize,
                PageNumber = pageNumber,
            });
        }

    }
}