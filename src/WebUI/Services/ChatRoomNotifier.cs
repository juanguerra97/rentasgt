using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.WebUI.Hubs;
using AutoMapper;
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;

namespace rentasgt.WebUI.Services
{
    public class ChatRoomNotifier : IChatRoomNotifier
    {
        private readonly IHubContext<ChatHub> chathubContext;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public ChatRoomNotifier(IHubContext<ChatHub> chathubContext, 
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            this.chathubContext = chathubContext;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task MessageRead(ChatMessage msg)
        {
            await this.chathubContext.Clients.User(this.currentUserService.UserId).SendAsync("messageReadNotification", this.mapper.Map<ChatMessageDto>(msg));
        }

    }
}
