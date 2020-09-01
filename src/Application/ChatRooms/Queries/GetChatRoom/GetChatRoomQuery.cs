using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.ChatRooms.Queries.GetChatRoomsOfUser;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;

namespace Application.ChatRooms.Queries.GetChatRoom
{
    public class GetChatRoomQuery : IRequest<ChatRoomDto>
    {

        public long ProductId { get; set; }

    }

    public class GetChatRoomQueryHandler : IRequestHandler<GetChatRoomQuery, ChatRoomDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetChatRoomQueryHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            this.mapper = mapper;
            this.currentUserService = currentUserService;
            this.context = context;

        }

        public async Task<ChatRoomDto> Handle(GetChatRoomQuery request, CancellationToken cancellationToken)
        {
            var userId = this.currentUserService.UserId;
            var entity = await this.context.ChatRooms
                .Include(r => r.User)
                .Include(r => r.Product).ThenInclude(p => p.Owner)
                .FirstOrDefaultAsync(r => (r.UserId == userId || r.Product.Owner.Id == userId) && r.ProductId == request.ProductId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ChatRoom), request.ProductId);
            }

            return this.mapper.Map<ChatRoomDto>(entity);
        }
    }

}