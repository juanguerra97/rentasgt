using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.DB;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Reports.Queries.RentsByYear
{
    public class RentsByYearQuery : IRequest<RentsByYearReport>
    {
    }

    public class RentsByYearQueryHandler : IRequestHandler<RentsByYearQuery, RentsByYearReport>
    {
        private readonly IApplicationDbContext context;

        public RentsByYearQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<RentsByYearReport> Handle(RentsByYearQuery request, CancellationToken cancellationToken)
        {
            var query = this.context.Rents
                .Where(r => r.Status == RentStatus.ProductReturned)
                .GroupBy(r => CustomDbFunctions.ExtractYear(r.EndDate))
                .Select(r => new YearResult { 
                    Year = r.Key,
                    Total = r.Count()
                })
                .OrderBy(r => r.Year);
            return new RentsByYearReport
            {
                Results = await query.ToListAsync()
            };
        }
    }

}
