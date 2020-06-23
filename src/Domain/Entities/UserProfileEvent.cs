using System;
using rentasgt.Domain.Enums;

namespace rentasgt.Domain.Entities
{
    public class UserProfileEvent
    {

        public UserProfileEvent()
        { }

        public UserProfileEvent(UserProfileEventType eventType, DateTime eventDate, 
            AppUser userProfile, AppUser userEvent, string? message = null)
        {
            EventType = eventType;
            EventDate = eventDate;
            UserProfile = userProfile;
            UserEvent = userEvent;
            Message = message;
        }

        public long Id { get; set; }

        public UserProfileEventType EventType { get; set; }

        public DateTime EventDate { get; set; }

        public string UserProfileId { get; set; }
        public AppUser UserProfile { get; set; } // profile on which the event happens

        public string UserEventId { get; set; }

        public AppUser UserEvent { get; set; } // user that triggers the event

        public string? Message { get; set; }

    }
}
