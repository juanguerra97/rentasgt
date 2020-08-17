using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Rents.Commands.EndRent
{
    public class EndRentCommand : IRequest
    {
        public long RentId { get; set; }
    }

    public class EndRentCommandHandler : IRequestHandler<EndRentCommand>
    {

        public IApplicationDbContext context;
        public ICurrentUserService currentUserService;
        private readonly IDateTime timeService;

        public EndRentCommandHandler(IApplicationDbContext context, 
            ICurrentUserService currentUserService,
            IDateTime timeService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.timeService = timeService;
        }

        public async Task<Unit> Handle(EndRentCommand request, CancellationToken cancellationToken)
        {

            var entity = await this.context.Rents
                .Include(r => r.Request)
                .ThenInclude(rq => rq.Product)
                .ThenInclude(p => p.Owner)
                .SingleOrDefaultAsync(r => r.RequestId == request.RentId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Rent), request.RentId);
            }

            if (entity.Status != RentStatus.ProductDelivered && entity.Status != RentStatus.ReturnDelayed)
            {
                throw new InvalidStateException($"No se puede terminar una renta en estado {entity.Status}");
            }

            var userId = this.currentUserService.UserId;
            if (userId != entity.Request.Product.Owner.Id)
            {
                throw new OperationForbidenException();
            }

            entity.Status = RentStatus.ProductReturned;
            entity.EndDate = this.timeService.Now;
            entity.TotalCost = entity.Request.EstimatedCost + CalculateFine(entity);

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private decimal CalculateFine(Rent rent)
        {
            decimal total = 0;
            if (rent.EndDate > rent.Request.EndDate)
            {
                var span = (rent.EndDate - rent.Request.EndDate).Value;
                var days = span.Days;
                var months = days / 31;
                if (rent.Request.Product.CostPerMonth != null)
                {
                    total += months * (decimal)rent.Request.Product.CostPerMonth;
                    days = days - months * 31;
                }
                var weeks = days / 7;
                if (rent.Request.Product.CostPerWeek != null)
                {
                    total += weeks * (decimal)rent.Request.Product.CostPerWeek;
                    days = days - weeks * 7;
                }
                total += days * rent.Request.Product.CostPerDay;
            }
            return total;
        }

    }

}
