using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace Application.RentRequests.Queries.GetReservedDatesForNextMonth
{
    public class GetReservedDatesForNextMonthQuery : IRequest<List<DateTime>>
    {

        public long ProductId { get; set; }

    }

    public class GetReservedDatesForNextMonthQueryHandler : IRequestHandler<GetReservedDatesForNextMonthQuery, List<DateTime>>
    {
        private readonly IApplicationDbContext context;
        private readonly IDateTime dateTimeService;

        public GetReservedDatesForNextMonthQueryHandler(IApplicationDbContext context, IDateTime dateTimeService)
        {
            this.dateTimeService = dateTimeService;
            this.context = context;
        }

        public async Task<List<DateTime>> Handle(GetReservedDatesForNextMonthQuery request, CancellationToken cancellationToken)
        {
            var prod = await this.context.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId);
            var dates = new List<DateTime>();
            if (prod == null)
            {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }
            var rangeStartDate = this.dateTimeService.Now.Date;
            var rangeEndDate = rangeStartDate.AddMonths(1).Date;
            var productRentRequests = await this.context.RentRequests
                .Include(r => r.Rent)
                .Where(r => r.Product.Id == prod.Id 
                    &&  r.StartDate >= rangeStartDate 
                    && r.StartDate <= rangeEndDate
                    && r.Status == RequestStatus.Accepted)
                .ToListAsync();
            productRentRequests = productRentRequests
                .Where(r => r.Rent != null 
                    && (r.Rent.Status == RentStatus.Pending || r.Rent.Status == RentStatus.ProductDelivered))
                    .OrderBy(r => r.StartDate)
                .ToList();
            foreach(var rentRequest in productRentRequests) 
            {
                dates.AddRange(GetDatesInRange(rentRequest.StartDate.Date, rentRequest.EndDate.Date));
            }
            
            return dates;
        }

        public static List<DateTime> GetDatesInRange(DateTime startDate, DateTime endDate)
        {
            var datesInRange = new List<DateTime>();
            var currentDate = startDate;
            while(currentDate <= endDate)
            {
                datesInRange.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }
            return datesInRange;
        }

    }

}