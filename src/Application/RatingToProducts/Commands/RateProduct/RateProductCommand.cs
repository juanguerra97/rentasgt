using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RatingToProducts.Commands.RateProduct
{
    public class RateProductCommand : IRequest<double>
    {
        public long RatingId { get; set; }
        public int ProductRatingValue { get; set; }
        public int OwnerRatingValue { get; set; }
        public string? Comment { get; set; }
    }

    public class RateProductCommandHandler : IRequestHandler<RateProductCommand, double>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;

        public RateProductCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTime = dateTime;
        }

        public async Task<double> Handle(RateProductCommand request, CancellationToken cancellationToken)
        {
            var userId = this.currentUserService.UserId;
            var entity = await this.context.RatingToProducts
                .Include(r => r.Product)
                .ThenInclude(p => p.Owner)
                .FirstOrDefaultAsync(r => r.Id == request.RatingId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(RatingToProduct), request.RatingId);
            }

            if (entity.FromUserId != userId)
            {
                throw new OperationForbidenException();
            }

            if (entity.Status != RatingStatus.Pending)
            {
                throw new InvalidStateException("Ya no se puede calificar el producto");
            }

            var product = await this.context.Products
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == entity.ProductId);
            var owner = await this.context.AppUsers
                .FirstOrDefaultAsync(u => u.Id == product.Owner.Id);

            var userRatingsQuery = this.context.RatingToUsers
                .Where(r => r.Status == RatingStatus.Rated && r.ToUserId == owner.Id)
                .Select(r => r.RatingValue);
            var ownerRatingsQuery = this.context.RatingToProducts
                .Include(r => r.Product.Owner)
                .Where(r => r.Status == RatingStatus.Rated && r.Product.Owner.Id == owner.Id)
                .Select(r => r.OwnerRatingValue);
            var productRatingsQuery = this.context.RatingToProducts
                .Where(r => r.Status == RatingStatus.Rated && r.ProductId == entity.ProductId)
                .Select(r => r.ProductRatingValue);

            var totalSum = userRatingsQuery.Sum() ?? 0;
            totalSum += (ownerRatingsQuery.Sum() ?? 0);
            var totalCount = userRatingsQuery.Count() + ownerRatingsQuery.Count();

            totalSum += request.OwnerRatingValue;
            totalCount += 1;

            owner.Reputation = totalSum / (double)totalCount;

            product.Rating = (((productRatingsQuery.Sum() ?? 0) + request.ProductRatingValue)) / (productRatingsQuery.Count() + 1.0);
            entity.Status = RatingStatus.Rated;
            entity.ProductRatingValue = request.ProductRatingValue;
            entity.OwnerRatingValue = request.OwnerRatingValue;

            await this.context.SaveChangesAsync(cancellationToken);

            return (double)entity.ProductRatingValue;
        }
    }

}
