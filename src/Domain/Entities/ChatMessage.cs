using rentasgt.Domain.Enums;
using System;

namespace rentasgt.Domain.Entities
{
    public class ChatMessage
    {

        public static readonly int MAX_MESSAGE_LENGTH = 512;

        public long Id { get; set; }

        /// <summary>
        /// Message status
        /// </summary>
        public ChatMessageStatus Status { get; set; }

        /// <summary>
        /// Chat room the message belongs to
        /// </summary>
        public long RoomId { get; set; }
        public ChatRoom Room { get; set; }

        /// <summary>
        /// The sender of the message
        /// </summary>
        public AppUser Sender { get; set; }

        /// <summary>
        /// Type of message
        /// </summary>
        public ChatMessageType MessageType { get; set; }

        /// <summary>
        /// Date and time when the message was sent
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Message's content
        /// </summary>
        public string Content { get; set; }

    }
}
