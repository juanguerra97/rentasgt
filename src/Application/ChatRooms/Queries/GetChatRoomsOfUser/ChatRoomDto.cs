using System.Linq;
using AutoMapper;
using rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom;
using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;
using System.Collections.Generic;

namespace rentasgt.Application.ChatRooms.Queries.GetChatRoomsOfUser
{
    public class ChatRoomDto : IMapFrom<ChatRoom>
    {

        public long Id { get; set; }
        public ChatRoomProductDto Product { get; set; }
        public ChatUserDto User { get; set; }
        public ChatMessageDto? LastMessage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChatRoom, ChatRoomDto>()
                .ForMember(d => d.LastMessage, opt => opt.MapFrom(s => s.Messages.OrderByDescending(s => s.SentDate).FirstOrDefault()));
        }

    }
}
