

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace Application.ChatRooms.Commands.ReadMessage {

    public class ReadMessageCommand: IRequest<ChatMessageDto> {
        public long MessageId { get; set;}
    }

    public class ReadMessageCommandHandler : IRequestHandler<ReadMessageCommand, ChatMessageDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IChatRoomNotificator chatRoomNotificator;
        private readonly IMapper mapper;

        public ReadMessageCommandHandler(IApplicationDbContext context, 
                ICurrentUserService currentUserService, 
                IChatRoomNotificator chatRoomNotificator,
                IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.chatRoomNotificator = chatRoomNotificator;
            this.mapper = mapper;
        }

        public async Task<ChatMessageDto> Handle(ReadMessageCommand request, CancellationToken cancellationToken)
        {
            
            var entity = await  this.context.ChatMessages
                .Include(m => m.Room)
                .ThenInclude(r => r.Product)
                .ThenInclude(p => p.Owner)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == request.MessageId);
                
            if (entity.Status == ChatMessageStatus.Leido) {
                throw new InvalidStateException("No puedes leer el mismo mensaje dos veces");
            }

            if (entity == null) {
                throw new NotFoundException(nameof(ChatMessage), request.MessageId);
            }

            var userId = this.currentUserService.UserId;

            if (userId != entity.Room.UserId && userId != entity.Room.Product.Owner.Id) {
                throw new OperationForbidenException("No perteneces a esta sala");
            }

            if (userId == entity.Sender.Id) {
                throw new OperationForbidenException("No puedes leer tu propio mensaje");
            }

            entity.Status = ChatMessageStatus.Leido;
            await this.context.SaveChangesAsync(cancellationToken);
            this.chatRoomNotificator.MessageRead(entity);
            return this.mapper.Map<ChatMessageDto>(entity);

        }
    }


}