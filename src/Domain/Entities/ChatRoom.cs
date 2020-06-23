using System.Collections.Generic;

namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Chat room's information
    /// </summary>
    public class ChatRoom
    {

        public ChatRoom()
        {
            Messages = new List<ChatMessage>();
            Users = new List<UserChatRoom>();
        }

        public long Id { get; set; }

        /// <summary>
        /// Users in the chat room
        /// </summary>
        public List<UserChatRoom> Users { get; set; }

        /// <summary>
        /// List of messages between the users in the chat room
        /// </summary>
        public List<ChatMessage> Messages { get; set; }

    }
}
