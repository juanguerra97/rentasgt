using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;

namespace rentasgt.Application.RentRequests.Queries.GetRentRequests
{
    public class RentRequestDto : IMapFrom<RentRequest>
    {

        public long Id { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Place { get; set; }
        public decimal EstimatedCost { get; set; }
        public RentRequestProductOwnerDto Requestor { get; set; }
        public RentRequestProductDto Product { get; set; }
        public List<RequestEventDto> Events { get; set; }


    }

}
