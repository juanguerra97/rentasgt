

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.WebUI.Hubs;

namespace rentasgt.WebUI.Services
{

    public class ChatRoomNotificator : IChatRoomNotificator
    {
        private readonly IHubContext<ChatHub> chatHubContext;
        private readonly IMapper mapper;

        public ChatRoomNotificator(IHubContext<ChatHub> chatHubContext, IMapper mapper)
        {
            this.chatHubContext = chatHubContext;
            this.mapper = mapper;
        }

        public async Task MessageRead(ChatMessage message)
        {
            await this.chatHubContext.Clients.User(message.Sender.Id).SendAsync("messageReadNotification", this.mapper.Map<ChatMessageDto>(message));
        }
    }

}