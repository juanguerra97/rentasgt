using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Conflicts.Commands.FinishConflict
{
    public class FinishConflictCommand : IRequest
    {
        public long ConflictId { get; set; }
    }

    public class FinishConflictCommandHandler : IRequestHandler<FinishConflictCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;

        public FinishConflictCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(FinishConflictCommand request, CancellationToken cancellationToken)
        {

            var entity = await this.context.Conflicts
                .FirstOrDefaultAsync(c => c.Id == request.ConflictId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Conflict), request.ConflictId);
            }

            var userId = this.currentUserService.UserId;
            if (entity.ModeratorId != userId)
            {
                throw new OperationForbidenException("Solo el moderador del reporte lo puede finalizar");
            }

            if (entity.Status != ConflictStatus.InProcess)
            {
                throw new InvalidStateException("Este reporte no puede ser marcado como finalizado");
            }
            
            entity.Status = ConflictStatus.Finished;

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
