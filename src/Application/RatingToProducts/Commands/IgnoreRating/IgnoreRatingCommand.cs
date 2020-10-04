using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace Application.RatingToProducts.Commands.IgnoreRating
{
    public class IgnoreRatingCommand : IRequest
    {
        public long RatingId { get; set; }
    }

    public class IgnoreRatingCommandHandler : IRequestHandler<IgnoreRatingCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;

        public IgnoreRatingCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(IgnoreRatingCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.RatingToProducts
                .FirstOrDefaultAsync(r => r.Id == request.RatingId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(RatingToProduct), request.RatingId);
            }

            var userId = this.currentUserService.UserId;
            if (entity.FromUserId != userId)
            {
                throw new OperationForbidenException();
            }

            if (entity.Status == RatingStatus.Rated)
            {
                throw new InvalidStateException("Este rating ya no puede ser modificado");
            }

            entity.Status = RatingStatus.Ignored;
            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}