using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Application.ConflictRecords.Queries.GetConflictRecords
{
    public class ConflictRecordDto : IMapFrom<ConflictRecord>
    {

        public long Id { get; set; }
        public long ConflictId { get; set; }
        public string Description { get; set; }
        public DateTime RecordDate { get; set; }

    }
}
