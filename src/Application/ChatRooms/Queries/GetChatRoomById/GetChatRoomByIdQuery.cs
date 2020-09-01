using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.ChatRooms.Queries.GetChatRoomsOfUser;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.ChatRooms.Queries.GetChatRoomById
{
    public class GetChatRoomByIdQuery : IRequest<ChatRoomDto>
    {
        public long RoomId { get; set; }
    }

    public class GetChatRoomByIdQueryHandler: IRequestHandler<GetChatRoomByIdQuery, ChatRoomDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetChatRoomByIdQueryHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            this.mapper = mapper;
            this.currentUserService = currentUserService;
            this.context = context;

        }

        public async Task<ChatRoomDto> Handle(GetChatRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = this.currentUserService.UserId;
            var entity = await this.context.ChatRooms
                .Include(r => r.User)
                .Include(r => r.Product).ThenInclude(p => p.Owner)
                .FirstOrDefaultAsync(r => (r.UserId == userId || r.Product.Owner.Id == userId) && r.Id == request.RoomId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ChatRoom), request.RoomId);
            }

            return this.mapper.Map<ChatRoomDto>(entity);
        }
    }

}
