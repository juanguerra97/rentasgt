using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RentRequests.Commands.ViewRentRequest
{
    public class ViewRentRequestCommand : IRequest
    {
        
        public long RentRequestId { get; set; }

    }

    public class ViewRentRequestCommandHandler : IRequestHandler<ViewRentRequestCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTimeService;

        public ViewRentRequestCommandHandler(IApplicationDbContext context, 
            ICurrentUserService currentUserService, IDateTime dateTimeService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTimeService = dateTimeService;
        }

        public async Task<Unit> Handle(ViewRentRequestCommand request, CancellationToken cancellationToken)
        {

            var entity = await context.RentRequests.Include(rq => rq.Product).ThenInclude(p => p.Owner).SingleOrDefaultAsync(rq => rq.Id == request.RentRequestId);
            if (entity == null)
            {
                throw new NotFoundException(nameof(RentRequest), request.RentRequestId);
            }

            if (entity.Status != RequestStatus.Pending)
            {
                throw new InvalidStateException("La solicitud ya no puede ser marcada como vista");
            }

            if (currentUserService.UserId != entity.Product.Owner.Id)
            {
                throw new OperationForbidenException("No puedes marcar la solicitud como vista");
            }

            entity.Status = RequestStatus.Viewed;
            entity.Events.Add(new RequestEvent
            {
                EventDate = dateTimeService.Now,
                EventType = RequestEventType.RequestViewed
            });

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
