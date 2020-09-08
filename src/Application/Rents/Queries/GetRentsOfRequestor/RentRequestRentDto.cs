using System;
using System.Collections.Generic;
using rentasgt.Application.Common.Mappings;
using rentasgt.Application.RentRequests.Queries.GetRentRequests;
using rentasgt.Application.Rents.Queries.GetRentsOfRequestor;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace Application.Rents.Queries.GetRentsOfRequestor
{
    public class RentRequestRentDto : IMapFrom<RentRequest>
    {
         public long Id { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Place { get; set; }
        public decimal EstimatedCost { get; set; }
        public RentDto Rent { get; set;}
        public RentRequestProductOwnerDto Requestor { get; set; }
        public RentRequestProductDto Product { get; set; }
        public List<RequestEventDto> Events { get; set; }
    }
}