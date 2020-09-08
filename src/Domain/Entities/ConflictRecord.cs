using System;

namespace rentasgt.Domain.Entities
{
    public class ConflictRecord
    {

        public static readonly int MAX_DESCRIPTION_LENGTH = 1024;

        public long Id { get; set; }

        public long ConflictId { get; set; }
        public Conflict Conflict { get; set; }
        public string Description { get; set; }
        public DateTime RecordDate { get; set; }

    }
}
