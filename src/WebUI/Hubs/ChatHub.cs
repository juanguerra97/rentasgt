using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace rentasgt.WebUI.Hubs
{

    [Authorize]
    public class ChatHub : Hub
    {

        private CancellationToken cancellationToken = new CancellationTokenSource().Token;
        private readonly IApplicationDbContext context;
        private readonly IDateTime dateTimeService;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public ChatHub(IApplicationDbContext context, IDateTime dateTimeService,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            this.currentUserService = currentUserService;
            this.mapper = mapper;
            this.dateTimeService = dateTimeService;
            this.context = context;
        }

        public async Task<ChatMessageDto> SendMessage(long roomId, string content)
        {

            var userId = this.Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            (ChatMessageDto msg, string recipientId) = await this.CreateMessage(roomId, content, userId);

            if (msg != null)
            {
                await Clients.User(recipientId).SendAsync("receiveMessage", msg);
            }
            return msg;
        }

        public override Task OnConnectedAsync()
        {
            // string userId = this.Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            // TODO: send disconnect notification
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

            var message = new ChatMessage
            {
                Room = roomEntity,
                Content = content,
                Sender = sender,
                SentDate = this.dateTimeService.Now,
                Status = ChatMessageStatus.SinLeer,
                MessageType = ChatMessageType.Text
            };

            await this.context.ChatMessages.AddAsync(message);
            roomEntity.LastMessage = message;
            await this.context.SaveChangesAsync(this.cancellationToken);
            // message = await this.context.ChatMessages.FirstOrDefaultAsync(m => m.Id == message.Id);
            return (this.mapper.Map<ChatMessageDto>(message), recipient.Id);
        }

    }
}