using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RentRequests.Commands.RejectRentRequest
{
    public class RejectRentRequestCommand : IRequest
    {

        public long RentRequestId { get; set; }
        public string? Message { get; set; }

    }

    public class RejectRentRequestCommandHandler : IRequestHandler<RejectRentRequestCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTimeService;

        public RejectRentRequestCommandHandler(IApplicationDbContext context, 
            ICurrentUserService currentUserService, 
            IDateTime dateTimeService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTimeService = dateTimeService;
        }

        public async Task<Unit> Handle(RejectRentRequestCommand request, CancellationToken cancellationToken)
        {

            var entity = await context.RentRequests
                .Include(rq => rq.Product).ThenInclude(p => p.Owner)
                .SingleOrDefaultAsync(rq => rq.Id == request.RentRequestId); 
            if (entity == null)
            {
                throw new NotFoundException(nameof(RentRequest), request.RentRequestId);
            }

            if (entity.Status != RequestStatus.Viewed)
            {
                throw new InvalidOperationException("No se puede cancelar la solicitud");
            }

            if (entity.Product.Owner.Id != currentUserService.UserId)
            {
                throw new OperationForbidenException("No tienes permiso para rechazar esta solicitud");
            }

            entity.Status = RequestStatus.Rejected;
            entity.Events.Add(new RequestEvent
            {
                EventDate = dateTimeService.Now,
                EventType = RequestEventType.RequestRejected,
                Message = request.Message
                
            });

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
