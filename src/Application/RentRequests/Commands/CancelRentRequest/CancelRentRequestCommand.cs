using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RentRequests.Commands.CancelRentRequest
{
    public class CancelRentRequestCommand : IRequest
    {

        public long RentRequestId { get; set; }

    }

    public class CancelRentRequestCommandHandler : IRequestHandler<CancelRentRequestCommand>
    {

        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTimeService;

        public CancelRentRequestCommandHandler(IApplicationDbContext context, 
            ICurrentUserService currentUserService,
            IDateTime dateTimeService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTimeService = dateTimeService;
        }

        public async Task<Unit> Handle(CancelRentRequestCommand request, CancellationToken cancellationToken)
        {

            var entity = await this.context.RentRequests.Include(rq => rq.Requestor).SingleOrDefaultAsync(rq => rq.Id == request.RentRequestId);
            if (entity == null)
            {
                throw new NotFoundException(nameof(RentRequest), request.RentRequestId);
            }

            if(entity.Requestor.Id != currentUserService.UserId)
            {
                throw new OperationForbidenException("No tienes permiso para cancelar la solicitud");
            }

            if (entity.Status != RequestStatus.Pending && entity.Status != RequestStatus.Viewed)
            {
                throw new InvalidStateException("La solicitud no puede ser cancelada");
            }

            var currentDate = dateTimeService.Now;

            entity.Status = RequestStatus.Cancelled;
            entity.Events.Add(new RequestEvent { 
                EventDate = currentDate,
                EventType = RequestEventType.RequestCancelled
            });

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
    
}
