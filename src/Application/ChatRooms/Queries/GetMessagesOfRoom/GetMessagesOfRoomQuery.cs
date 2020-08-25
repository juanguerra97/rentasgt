using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Domain.Entities;

namespace Application.ChatRooms.Queries.GetMessagesOfRoom
{
    public class GetMessagesOfRoomQuery : IRequest<PaginatedListResponse<ChatMessageDto>>
    {
        
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public long ChatRoomId { get; set; }

    }

    public class GetMessagesOfRoomQueryHandler : IRequestHandler<GetMessagesOfRoomQuery, PaginatedListResponse<ChatMessageDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetMessagesOfRoomQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }
        public async Task<PaginatedListResponse<ChatMessageDto>> Handle(GetMessagesOfRoomQuery request, CancellationToken cancellationToken)
        {
            
            var entity = await this.context.ChatRooms
                .Include(c => c.User)
                .Include(c => c.Product)
                .ThenInclude(p => p.Owner)
                .FirstOrDefaultAsync(c => c.Id == request.ChatRoomId);
            if (entity == null) {
                throw new NotFoundException(nameof(ChatRoom), request.ChatRoomId);
            }
            var userId = this.currentUserService.UserId;
            if (userId != entity.User.Id && userId != entity.Product.Owner.Id) {
                throw new OperationForbidenException();
            }
            
            return PaginatedListResponse<ChatMessageDto>.ToPaginatedListResponse(
                this.context.ChatMessages.Where(m => m.RoomId == entity.Id).OrderByDescending(m => m.Id).ProjectTo<ChatMessageDto>(this.mapper.ConfigurationProvider),
                request.PageNumber, request.PageSize
            );
        }
    }

}