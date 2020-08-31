using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MediatR;
using AutoMapper;
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;
using rentasgt.Domain.Entities;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Exceptions;

namespace rentasgt.Application.ChatRooms.Commands.ReadMessage
{
    public class ReadMessageCommand : IRequest<ChatMessageDto>
    {

        public long MessageId { get; set; }

    }

    public class ReadMessageCommandHandler : IRequestHandler<ReadMessageCommand, ChatMessageDto>
    {
        private readonly IApplicationDbContext context;
        private readonly IChatRoomNotifier chatRoomNotifier;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public ReadMessageCommandHandler(IApplicationDbContext context, 
            IChatRoomNotifier chatRoomNotifier,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.chatRoomNotifier = chatRoomNotifier;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<ChatMessageDto> Handle(ReadMessageCommand request, CancellationToken cancellationToken)
        {
            var msgEntity = await this.context.ChatMessages
                    .Include(m => m.Sender)
                    .Include(m => m.Room)
                    .ThenInclude(r => r.Product)
                    .ThenInclude(p => p.Owner)
                    .Where(m => m.Id == request.MessageId)
                    .FirstOrDefaultAsync();
            var userId = this.currentUserService.UserId;

            if (msgEntity == null)
            {
                throw new NotFoundException(nameof(ChatMessage), request.MessageId);
            }

            if (userId != msgEntity.Room.UserId && userId != msgEntity.Room.Product.Owner.Id)
            {
                throw new OperationForbidenException("No perteneces a esta sala");
            }

            if (msgEntity.Sender.Id == userId)
            {
                throw new OperationForbidenException("No puedes leer tu propio mensaje");
            }

             await this.context.ChatMessages
                .Include(m => m.Sender)
                .Where(m => m.RoomId == msgEntity.RoomId)
                .Where(m => m.Sender.Id == msgEntity.Sender.Id)
                .Where(m => m.Status == Domain.Enums.ChatMessageStatus.SinLeer)
                .Where(m => m.SentDate < msgEntity.SentDate)
                .ForEachAsync(m =>
                {
                    m.Status = Domain.Enums.ChatMessageStatus.Leido;
                });

            msgEntity.Status = Domain.Enums.ChatMessageStatus.Leido;

            await this.context.SaveChangesAsync(cancellationToken);
            await this.chatRoomNotifier.MessageRead(msgEntity);
            return this.mapper.Map<ChatMessageDto>(msgEntity);
        }
    }

}
