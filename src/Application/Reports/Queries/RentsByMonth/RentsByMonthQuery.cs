using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.DB;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Reports.Queries.RentsByMonth
{
    public class RentsByMonthQuery : IRequest<RentsByMonthReport>
    {

        public int Year { get; set; }

    }

    public class RentsByMonthQueryHandler : IRequestHandler<RentsByMonthQuery, RentsByMonthReport>
    {
        private readonly IApplicationDbContext context;

        public RentsByMonthQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<RentsByMonthReport> Handle(RentsByMonthQuery request, CancellationToken cancellationToken)
        {
            var query = this.context.Rents
                .Where(r => r.Status == RentStatus.ProductReturned
                    && CustomDbFunctions.IsYear(r.EndDate, request.Year))
                .GroupBy(r => CustomDbFunctions.ExtractMonth(r.EndDate))
                .Select(g => new MonthResult 
                    { 
                        Month = g.Key,
                        Total = g.Count()
                    })
                .OrderBy(r => r.Month);
            return new RentsByMonthReport
            {
                Year = request.Year,
                Results = await query.ToListAsync()
            };
        }
    }

}
