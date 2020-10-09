using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.DB;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Reports.Queries.RentsByDay
{
    public class RentsByDayQuery : IRequest<RentsByDayReport>
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class RentsByDayQueryHandler : IRequestHandler<RentsByDayQuery, RentsByDayReport>
    {
        private readonly IApplicationDbContext context;

        public RentsByDayQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<RentsByDayReport> Handle(RentsByDayQuery request, CancellationToken cancellationToken)
        {
            var date = new DateTime(request.Year, request.Month, 1);
            var query = this.context.Rents
                .Where(r => r.Status == RentStatus.ProductReturned
                    && CustomDbFunctions.IsMonth(r.EndDate, date))
                .GroupBy(r => CustomDbFunctions.ExtractDay(r.EndDate))
                .Select(g => new DayResult
                {
                    Day = g.Key,
                    Total = g.Count()
                })
                .OrderBy(r => r.Day);

            return new RentsByDayReport
            {
                Year = request.Year,
                Month = request.Month,
                Results = await query.ToListAsync()
            };
        }
    }

}
