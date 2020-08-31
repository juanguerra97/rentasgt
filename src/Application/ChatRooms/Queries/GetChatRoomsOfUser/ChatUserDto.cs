using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.ChatRooms.Queries.GetChatRoomsOfUser
{
    public class ChatUserDto : IMapFrom<AppUser>
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
