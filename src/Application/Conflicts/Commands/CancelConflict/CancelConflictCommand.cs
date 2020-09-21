using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Conflicts.Commands.CancelConflict
{
    public class CancelConflictCommand : IRequest
    {
        public long ConflictId { get; set; }
    }

    public class CancelConflictCommandHandler : IRequestHandler<CancelConflictCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;

        public CancelConflictCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(CancelConflictCommand request, CancellationToken cancellationToken)
        {

            var entity = await this.context.Conflicts
                .FirstOrDefaultAsync(c => c.Id == request.ConflictId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Conflict), request.ConflictId);
            }

            var userId = this.currentUserService.UserId;
            if (entity.ComplainantId != userId)
            {
                throw new OperationForbidenException("No tienes permiso para cancelar el reporte");
            }

            if (entity.Status != ConflictStatus.Pending)
            {
                throw new InvalidStateException("Este reporte ya no puede ser cancelado");
            }

            entity.Status = ConflictStatus.Cancelled;

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
