using rentasgt.Domain.Enums;

namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Text chat messages
    /// </summary>
    public class TextMessage : ChatMessage
    {

        public TextMessage()
        {
            MessageType = ChatMessageType.Text;
        }

        public TextMessage(string textContent)
            : this()
        {
            TextContent = textContent;
        }

        /// <summary>
        /// Content of the message (text)
        /// </summary>
        public string TextContent { get; set; }

    }
}
