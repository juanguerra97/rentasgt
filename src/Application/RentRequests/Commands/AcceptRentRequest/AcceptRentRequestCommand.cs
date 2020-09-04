using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RentRequests.Commands.AcceptRentRequest
{
    public class AcceptRentRequestCommand : IRequest
    {

        public long RentRequestId { get; set; }

    }

    public class AcceptRentRequestCommandHandler : IRequestHandler<AcceptRentRequestCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<AppUser> userManager;
        private readonly IDateTime dateTimeService;

        public AcceptRentRequestCommandHandler(IApplicationDbContext context, 
            ICurrentUserService currentUserService,
            UserManager<AppUser> userManager,
            IDateTime dateTimeService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.userManager = userManager;
            this.dateTimeService = dateTimeService;
        }

        public async Task<Unit> Handle(AcceptRentRequestCommand request, CancellationToken cancellationToken)
        {

            var entity = await this.context.RentRequests
                .Include(rq => rq.Product).ThenInclude(p => p.Owner)
                .Include(rq => rq.Requestor)
                .SingleOrDefaultAsync(rq => rq.Id == request.RentRequestId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(RentRequest), request.RentRequestId);
            }

            if (entity.Status != RequestStatus.Viewed)
            {
                throw new InvalidStateException($"No se puede aceptar una solicitud en estado {entity.Status.ToString()}");
            }

            var currentUserId = currentUserService.UserId;

            if (entity.Product.Owner.Id != currentUserId)
            {
                throw new OperationForbidenException("Operacion no permitida");
            }

            var currentDate = dateTimeService.Now;
            if (entity.StartDate.CompareTo(currentDate) < 0)
            {
                throw new InvalidStateException("Hay conflicto con la fecha actual");
            }

            if (await this.context.RentRequests.Where(rq => rq.Status == RequestStatus.Accepted).Where(rq => (entity.StartDate.CompareTo(rq.StartDate) >= 0 && entity.StartDate.CompareTo(rq.EndDate) <= 0) || (entity.EndDate.CompareTo(rq.StartDate) >= 0 && entity.EndDate.CompareTo(rq.EndDate) <= 0) || (entity.StartDate.CompareTo(rq.StartDate) <= 0 && (entity.EndDate.CompareTo(rq.EndDate) >= 0))).AnyAsync())
            {
                throw new InvalidStateException("Hay conflicto con otra solicitud");
            }

            entity.Status = RequestStatus.Accepted;
            entity.Rent = new Rent
            {
                Status = RentStatus.Pending
            };
            entity.Events.Add(new RequestEvent
            {
                EventDate = currentDate,
                EventType = RequestEventType.RequestAccepted
            });

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
