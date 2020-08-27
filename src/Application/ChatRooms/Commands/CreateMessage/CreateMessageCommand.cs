using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;

namespace Application.ChatRooms.Commands.CreateMessage
{
    public class CreateMessageCommand : IRequest<ChatMessageDto>
    {

        public long RoomId { get; set; }
        public string Content { get; set; }

    }

    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, ChatMessageDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;
        private readonly IDateTime timeService;

        public CreateMessageCommandHandler(IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IDateTime timeService,
        IMapper mapper)
        {
            this.timeService = timeService;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
            this.context = context;
        }

        public async Task<ChatMessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.ChatRooms
                .Include(r => r.Product).ThenInclude(p => p.Owner)
                .FirstOrDefaultAsync(r => r.Id == request.RoomId);
            if (entity == null) 
            {
                throw new NotFoundException(nameof(ChatRoom), request.RoomId);
            }

            var userId = this.currentUserService.UserId;
            if (userId != entity.UserId && userId != entity.Product.Owner.Id) 
            {
                throw new OperationForbidenException("No puedes enviar mensajes en esta sala");
            }

            var newMessage = new ChatMessage {
                Room = entity,
                Content = request.Content,
                Sender = null
            };
            // INCOMPLETE!
            return this.mapper.Map<ChatMessageDto>(newMessage);

        }
    }

}