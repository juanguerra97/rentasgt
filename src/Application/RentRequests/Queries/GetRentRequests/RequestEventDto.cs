using AutoMapper;
using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;

namespace rentasgt.Application.RentRequests.Queries.GetRentRequests
{
    public class RequestEventDto : IMapFrom<RequestEvent>
    {

        public long Id { get; set; }
        public long RentRequestId { get; set; }
        public RequestEventType EventType { get; set; }
        public DateTime EventDate { get; set; }
        public string? Message { get; set; }

    }
}
