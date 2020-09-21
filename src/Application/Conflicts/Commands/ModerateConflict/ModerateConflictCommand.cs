using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Conflicts.Commands.ModerateConflict
{
    public class ModerateConflictCommand : IRequest
    {
        public long ConflictId { get; set; }
    }

    public class ModerateConflictCommandHandler : IRequestHandler<ModerateConflictCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;

        public ModerateConflictCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ModerateConflictCommand request, CancellationToken cancellationToken)
        {

            var entity = await this.context.Conflicts
                .Include(c => c.Rent.Request.Product.Owner)
                .FirstOrDefaultAsync(c => c.Id == request.ConflictId);
           
            if (entity == null)
            {
                throw new NotFoundException(nameof(Conflict), request.ConflictId);
            }

            if (entity.Status != ConflictStatus.Pending)
            {
                throw new InvalidStateException("Este reporte ya fué asignado a otro moderador");
            }

            var userId = this.currentUserService.UserId;
            if (entity.Rent.Request.RequestorId == userId 
                || entity.Rent.Request.Product.Owner.Id == userId)
            {
                throw new OperationForbidenException("Conflicto de intereses");
            }

            entity.Status = ConflictStatus.InProcess;
            entity.ModeratorId = userId;

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}