using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace WebUI.Hubs
{

    [Authorize]
    public class ChatHub : Hub
    {

        private CancellationToken cancellationToken = new CancellationTokenSource().Token;

        private readonly ConcurrentDictionary<string, string> ActiveUsersByConId = new ConcurrentDictionary<string, string>();
        private readonly ConcurrentDictionary<string, string> ActiveUsersByUserId = new ConcurrentDictionary<string, string>();
        private readonly IApplicationDbContext context;
        private readonly IDateTime dateTimeService;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public ChatHub(IApplicationDbContext context, IDateTime dateTimeService, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.mapper = mapper;
            this.currentUserService = currentUserService;
            this.dateTimeService = dateTimeService;
            this.context = context;
        }

        public async Task<ChatMessageDto> SendMessage(long roomId, string content)
        {

            var userId = this.Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            (ChatMessageDto msg, string recipientId) = await this.CreateMessage(roomId, content, userId);

            if (msg != null)
            {
                
                if (this.ActiveUsersByUserId.TryGetValue(recipientId, out string recipientConnectionId))
                {
                    await Clients.Client(recipientConnectionId).SendAsync("receiveMessage", msg);
                } else 
                {
                    Console.WriteLine("# Recipient is not online!");
                }
            } else 
            {
                Console.WriteLine("# Message not saved!");
            }
            return msg;
        }

        public override Task OnConnectedAsync()
        {
            string userId = this.Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            // this.ActiveUsersByConId.TryAdd(this.Context.ConnectionId, userId);
            if (this.ActiveUsersByUserId.ContainsKey(userId)) 
            {
                this.ActiveUsersByUserId.TryRemove(userId, out string v);
            }
            this.ActiveUsersByUserId.TryAdd(userId, this.Context.ConnectionId);
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            Console.WriteLine("# User disconnected");
            string userId = this.Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            this.ActiveUsersByUserId.TryRemove(userId, out string v);
            // this.ActiveUsersByConId.TryRemove(this.Context.ConnectionId, out string v);
            return Task.CompletedTask;
        }


        private async Task<(ChatMessageDto, string)> CreateMessage(long roomId, string content, string senderId)
        {
            var roomEntity = await this.context.ChatRooms
                .Include(r => r.User)
                .Include(r => r.Product)
                .ThenInclude(p => p.Owner).FirstOrDefaultAsync(r => r.Id == roomId);

            if (roomEntity == null || (senderId != roomEntity.UserId && senderId != roomEntity.Product.Owner.Id))
            {
                return (null, null);
            }

            var sender = roomEntity.User;
            var recipient = roomEntity.Product.Owner;
            if (roomEntity.UserId != senderId)
            {
                sender = roomEntity.Product.Owner;
                recipient = roomEntity.User;
            }

            var message = new ChatMessage {
                Room = roomEntity,
                Content = content,
                Sender = sender,
                SentDate = this.dateTimeService.Now,
                Status = ChatMessageStatus.SinLeer,
                MessageType = ChatMessageType.Text
            };

            await this.context.ChatMessages.AddAsync(message);
            await this.context.SaveChangesAsync(this.cancellationToken);

            return (this.mapper.Map<ChatMessageDto>(message), recipient.Id);
        }

    }
}