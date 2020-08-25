using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.ChatRooms.Queries.GetChatRoomsOfUser
{
    public class GetChatRoomsOfUserQuery : IRequest<PaginatedListResponse<ChatRoomDto>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    class GetChatRoomsOfUserQueryHandler : IRequestHandler<GetChatRoomsOfUserQuery, PaginatedListResponse<ChatRoomDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetChatRoomsOfUserQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<PaginatedListResponse<ChatRoomDto>> Handle(GetChatRoomsOfUserQuery request, CancellationToken cancellationToken)
        {

            var chatRooms = this.context.ChatRooms
                .Include(cr => cr.Messages)
                .Include(cr => cr.Product)
                .ThenInclude(p => p.Owner)
                .Include(cr => cr.User)
                .Where(c => c.UserId == this.currentUserService.UserId || c.Product.Owner.Id == this.currentUserService.UserId)
                .Where(c => c.Messages.Count > 0);

            return PaginatedListResponse<ChatRoomDto>.ToPaginatedListResponse(
                chatRooms.ProjectTo<ChatRoomDto>(this.mapper.ConfigurationProvider).OrderByDescending(c => c.LastMessage.Id), 
                request.PageNumber, request.PageSize);
        }
    }

}
