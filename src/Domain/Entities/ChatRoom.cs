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
        }

        public long Id { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        /// <summary>
        /// List of messages between the users in the chat room
        /// </summary>
        public List<ChatMessage> Messages { get; set; }

    }
}
