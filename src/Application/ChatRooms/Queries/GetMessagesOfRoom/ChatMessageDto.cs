using System;
using rentasgt.Application.ChatRooms.Queries.GetChatRoomsOfUser;
using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace rentasgt.Application.ChatRooms.Queries.GetMessagesOfRoom
{
    public class ChatMessageDto : IMapFrom<ChatMessage>
    {
        public long Id { get; set; }
        public long RoomId { get; set;}
        public ChatMessageStatus Status { get; set; }
        public ChatUserDto Sender { get; set; }
        public DateTime? SentDate { get; set; }
        public string Content { get; set; }

    }
}
