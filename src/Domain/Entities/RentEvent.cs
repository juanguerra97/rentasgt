using rentasgt.Domain.Enums;
using System;

namespace rentasgt.Domain.Entities
{
    public class RentEvent
    {
        public static readonly int MAX_MESSAGE_LENGTH = 128;

        public long Id { get; set; }
        public RentEventType EventType { get; set; }
        public long RentId { get; set; }
        public Rent Rent { get; set; }
        public DateTime EventDate { get; set; }
        public string? Message { get; set; }
    }
}
