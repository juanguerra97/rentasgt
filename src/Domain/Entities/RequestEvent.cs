using System;
using rentasgt.Domain.Enums;

namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Events associated with a rent request
    /// </summary>
    public class RequestEvent
    {

        public RequestEvent()
        { }

        public RequestEvent(RequestEventType eventType, RentRequest rentRequest,
            DateTime eventDate, string? message= null)
            : this()
        {
            EventType = eventType;
            RentRequest = rentRequest;
            EventDate = eventDate;
            Message = message;
        }

        public long Id { get; set; }

        /// <summary>
        /// Type of the event
        /// </summary>
        public RequestEventType EventType { get; set; }

        /// <summary>
        /// Request this event belongs to
        /// </summary>
        public long RentRequestId { get; set; }
        public RentRequest RentRequest { get; set; }

        /// <summary>
        /// Date and time of the event
        /// </summary>
        public DateTime EventDate { get; set; }

        public string? Message { get; set; }

    }
}
