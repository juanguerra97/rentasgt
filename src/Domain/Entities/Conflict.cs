using rentasgt.Domain.Common;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;

namespace rentasgt.Domain.Entities
{
    public class Conflict : AuditableEntity
    {

        public static readonly int MAX_DESCRIPTION_LENGTH = 1024;

        public long Id { get; set; }

        public ConflictStatus Status { get; set; }

        public long RentId { get; set; }
        public Rent Rent { get; set; }
        
        public string? ModeratorId { get; set; }
        public AppUser? Moderator { get; set; }

        public string Description { get; set; }

        public string ComplainantId { get; set; }
        public AppUser Complainant { get; set; }

        public DateTime ConflictDate { get; set; }

        public List<ConflictRecord> ConflictRecords { get; set; }

    }
}
