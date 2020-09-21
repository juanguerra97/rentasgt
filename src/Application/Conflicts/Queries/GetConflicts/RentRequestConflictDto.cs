using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;

namespace rentasgt.Application.Conflicts.Queries.GetConflicts
{
    public class RentRequestConflictDto : IMapFrom<RentRequest>
    {
        public long Id { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public ProductConflictDto Product { get; set; }
        public string RequestorId { get; set; }
        public UserConflictDto Requestor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Place { get; set; }
        public decimal EstimatedCost { get; set; }

    }
}
