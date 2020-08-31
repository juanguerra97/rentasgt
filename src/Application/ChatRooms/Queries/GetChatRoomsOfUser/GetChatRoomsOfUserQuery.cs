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
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;

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

            var chatRooms = await this.context.ChatRooms
                .Include(cr => cr.Product)
                .ThenInclude(p => p.Owner)
                .Include(cr => cr.User)
                .Include(cr => cr.LastMessage)
                .Where(c => c.UserId == this.currentUserService.UserId || c.Product.Owner.Id == this.currentUserService.UserId)
                .Where(c => c.LastMessage != null)
                .ProjectTo<ChatRoomDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            chatRooms = chatRooms.OrderByDescending(c => c.LastMessage, new MessageDtoComp()).ToList();

            var pageSize = request.PageSize;
            var pageNumber = request.PageNumber;
            var count = chatRooms.Count();
			var items = chatRooms.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                
            return new PaginatedListResponse<ChatRoomDto>(
                items, count, pageNumber, pageSize
            );
        }
    }

    public class MessageDtoComp : IComparer<ChatMessageDto> {

        public int Compare(ChatMessageDto m1, ChatMessageDto m2) {
            return DateTime.Compare((DateTime)m1.SentDate, (DateTime) m2.SentDate);
        }

    }


}
