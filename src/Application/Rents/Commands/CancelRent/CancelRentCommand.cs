using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Rents.Commands.CancelRent
{
    public class CancelRentCommand : IRequest
    {
        public long RentId { get; set; }
    }

    public class CancelRentCommandHandler : IRequestHandler<CancelRentCommand>
    {

        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;

        public CancelRentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(CancelRentCommand request, CancellationToken cancellationToken)
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

            if (entity.Status != RentStatus.Pending)
            {
                throw new InvalidStateException($"No se puede cancelar una renta en estado {entity.Status}");
            }

            var userId = this.currentUserService.UserId;
            if (userId != entity.Request.RequestorId)
            {
                throw new OperationForbidenException("No puedes cancelar esta renta");
            }

            entity.Status = RentStatus.Cancelled;
            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
