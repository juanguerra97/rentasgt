using rentasgt.Application.Common.Mappings;
using rentasgt.Application.RentRequests.Queries.GetRentRequests;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;

namespace rentasgt.Application.Rents.Queries.GetRentsOfRequestor
{
    public class RentDto : IMapFrom<Rent>
    {
        public long RequestId { get; set; }
        public RentStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
