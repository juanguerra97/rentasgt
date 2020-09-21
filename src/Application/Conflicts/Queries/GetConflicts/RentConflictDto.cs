using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;

namespace rentasgt.Application.Conflicts.Queries.GetConflicts
{
    public class RentConflictDto : IMapFrom<Rent>
    {
        public long RequestId { get; set; }
        public RentRequestConflictDto Request { get; set; }
        public RentStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
