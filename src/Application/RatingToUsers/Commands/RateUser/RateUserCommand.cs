using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RatingToUsers.Commands.RateUser
{
    public class RateUserCommand : IRequest<double>
    {
        public long RatingId { get; set; }
        public int RatingValue { get; set; }
    }

    public class RateUserCommandHandler : IRequestHandler<RateUserCommand, double>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;

        public RateUserCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTime = dateTime;
        }

        public async Task<double> Handle(RateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.RatingToUsers
                .Include(r => r.ToUser)
                .FirstOrDefaultAsync(r => r.Id == request.RatingId);
            var userId = this.currentUserService.UserId;
            if (entity == null)
            {
                throw new NotFoundException(nameof(RatingToUser), request.RatingId);
            }

            if (entity.FromUserId != userId)
            {
                throw new OperationForbidenException();
            }

            if (entity.Status != RatingStatus.Pending)
            {
                throw new InvalidStateException("Ya no se puede calificar al usuario");
            }

            var userRatingsQuery = this.context.RatingToUsers
                .Where(r => r.Status == RatingStatus.Rated && r.ToUserId == entity.ToUserId)
                .Select(r => r.RatingValue);
            var ownerRatingsQuery = this.context.RatingToProducts
                .Include(r => r.Product.Owner)
                .Where(r => r.Status == RatingStatus.Rated && r.Product.Owner.Id == entity.ToUserId)
                .Select(r => r.OwnerRatingValue);

            var totalSum = userRatingsQuery.Sum() ?? 0;
            totalSum += (ownerRatingsQuery.Sum() ?? 0);
            var totalCount = userRatingsQuery.Count() + ownerRatingsQuery.Count();

            totalSum += request.RatingValue;
            totalCount += 1;

            entity.RatingValue = request.RatingValue;
            entity.RatingDate = this.dateTime.Now;
            entity.Status = RatingStatus.Rated;

            entity.ToUser.Reputation = totalSum / totalCount;

            await this.context.SaveChangesAsync(cancellationToken);

            return (double)entity.ToUser.Reputation;
        }
    }

}