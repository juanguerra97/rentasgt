using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Rents.Commands.StartRent
{
    public class StartRentCommand : IRequest
    {
        public long RentId { get; set; }
    }

    public class StartRentCommandHandler : IRequestHandler<StartRentCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly IDateTime timeService;
        private readonly ICurrentUserService currentUserService;

        public StartRentCommandHandler(IApplicationDbContext context,
            IDateTime timeService, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.timeService = timeService;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(StartRentCommand request, CancellationToken cancellationToken)
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

            var userId = this.currentUserService.UserId;
            if (userId != entity.Request.RequestorId)
            {
                throw new OperationForbidenException("No puedes iniciar esta renta");
            }

            if (entity.Status != RentStatus.Pending)
            {
                throw new InvalidStateException($"No se puede iniciar una renta en estado {RentStatus.Pending.ToString()}");
            }

            var today = this.timeService.Now.Date;
            if (today.CompareTo(entity.Request.StartDate.Date) < 0) {
                throw new InvalidStateException($"La renta debe empezar el {entity.Request.StartDate.ToString("dd/MM/yyyy")}");
            }
            
            entity.StartDate = this.timeService.Now;
            entity.Status = RentStatus.ProductDelivered;
            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
