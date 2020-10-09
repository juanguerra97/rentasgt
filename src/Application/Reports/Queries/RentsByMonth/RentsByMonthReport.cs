using System.Collections.Generic;

namespace rentasgt.Application.Reports.Queries.RentsByMonth
{
    public class RentsByMonthReport
    {
        public int Year { get; set; }
        public List<MonthResult> Results { get; set; }
    }
}