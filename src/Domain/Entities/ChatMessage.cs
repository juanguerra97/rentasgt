using rentasgt.Domain.Enums;
using System;

namespace rentasgt.Domain.Entities
{
    public class ChatMessage
    {

        public ChatMessage()
        { }

        public ChatMessage(ChatMessageStatus status, ChatRoom room, AppUser sender, AppUser recipient,
            ChatMessageType messageType, DateTime sentDate)
            : this()
        {
            Status = status;
            Room = room;
            Sender = sender;
            Recipient = recipient;
            MessageType = messageType;
            SentDate = sentDate;
        }

        public ChatMessage(ChatMessageStatus status, ChatRoom room, AppUser sender, AppUser recipient,
            ChatMessageType messageType)
            : this()
        {
            Status = status;
            Room = room;
            Sender = sender;
            Recipient = recipient;
            MessageType = messageType;
        }

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
        /// The recipient of the message
        /// </summary>
        public AppUser Recipient { get; set; }

        /// <summary>
        /// Type of message
        /// </summary>
        public ChatMessageType MessageType { get; set; }

        /// <summary>
        /// Date and time when the message was sent
        /// </summary>
        public DateTime? SentDate { get; set; }

    }
}
