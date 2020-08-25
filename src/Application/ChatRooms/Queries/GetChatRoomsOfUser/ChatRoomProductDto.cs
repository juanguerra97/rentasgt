using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.ChatRooms.Queries.GetChatRoomsOfUser
{
    public class ChatRoomProductDto : IMapFrom<Product>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ChatUserDto Owner { get; set; }
    }
}
