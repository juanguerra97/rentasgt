using rentasgt.Application.Common.Mappings;
using rentasgt.Application.ConflictRecords.Queries.GetConflictRecords;
using rentasgt.Application.Rents.Queries.GetRentsOfRequestor;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Application.Conflicts.Queries.GetConflicts
{
    public class ConflictDto : IMapFrom<Conflict>
    {

        public long Id { get; set; }

        public ConflictStatus Status { get; set; }

        public long RentId { get; set; }
        public RentConflictDto Rent { get; set; }

        public string? ModeratorId { get; set; }
        public UserConflictDto? Moderator { get; set; }

        public string Description { get; set; }

        public string ComplainantId { get; set; }
        public UserConflictDto Complainant { get; set; }

        public DateTime ConflictDate { get; set; }

        public List<ConflictRecordDto> ConflictRecords { get; set; }

    }
}
