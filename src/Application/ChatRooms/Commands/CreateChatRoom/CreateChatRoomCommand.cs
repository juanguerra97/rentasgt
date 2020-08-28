using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using System.Linq;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace Application.ChatRooms.Commands.CreateChatRoom
{
    public class CreateChatRoomCommand : IRequest<long>
    {
        public long ProductId { get; set; }
        public string FirstMessage { get; set; }
    }

    public class CreateChatRoomCommandHandler : IRequestHandler<CreateChatRoomCommand, long>
    {
        private readonly IApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime currentTimeService;

        public CreateChatRoomCommandHandler(IApplicationDbContext context,
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            IDateTime currentTimeService)
        {
            this.currentUserService = currentUserService;
            this.currentTimeService = currentTimeService;
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<long> Handle(CreateChatRoomCommand request, CancellationToken cancellationToken)
        {
            
            var productEntity = await this.context.Products
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId);
            if (productEntity == null)
            {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }
            var user = await this.userManager.FindByIdAsync(this.currentUserService.UserId);
            if (productEntity.Owner.Id == user.Id)
            {
                throw new OperationForbidenException("No puedes chatear contigo mismo");
            }

            var room = new ChatRoom {
                Product = productEntity,
                User = user,
                Messages = new List<ChatMessage> {
                    new ChatMessage {
                        Content = request.FirstMessage,
                        MessageType = ChatMessageType.Text,
                        Status = ChatMessageStatus.SinLeer,
                        Sender = user,
                        SentDate = currentTimeService.Now
                    }
                }
            };

            await this.context.ChatRooms.AddAsync(room);
            await this.context.SaveChangesAsync(cancellationToken);
            room.LastMessage = room.Messages.FirstOrDefault();
            await this.context.SaveChangesAsync(cancellationToken);

            return room.Id;
        }
    }

}