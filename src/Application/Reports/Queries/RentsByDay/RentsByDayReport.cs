using System.Collections.Generic;

namespace rentasgt.Application.Reports.Queries.RentsByDay
{
    public class RentsByDayReport
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<DayResult> Results { get; set; }
    }
}
