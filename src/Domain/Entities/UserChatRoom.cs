
namespace rentasgt.Domain.Entities
{
    public class UserChatRoom
    {

        public UserChatRoom()
        { }

        public UserChatRoom(AppUser user, ChatRoom room)
        {
            User = user;
            Room = room;
        }

        public UserChatRoom(ChatRoom room, AppUser user)
            : this(user, room)
        { }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public long RoomId { get; set; }
        public ChatRoom Room { get; set; }

    }
}
