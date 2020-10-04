using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RatingToProducts.Queries.GetPendingRatingToProduct
{
    public class GetPendingRatingToProductQuery : IRequest<RatingToProductDto>
    {
    }

    public class GetPendingRatingToProductQueryHandler : IRequestHandler<GetPendingRatingToProductQuery, RatingToProductDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetPendingRatingToProductQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<RatingToProductDto> Handle(GetPendingRatingToProductQuery request, CancellationToken cancellationToken)
        {
            var userId = this.currentUserService.UserId;
            var pendingRating = await this.context.RatingToProducts
                .Include(r => r.Product.Owner)
                .ThenInclude(o => o.ProfilePicture.Picture)
                .Include(r => r.Product.Pictures).ThenInclude(p => p.Picture)
                .Include(r => r.FromUser)
                .ThenInclude(u => u.ProfilePicture.Picture)
                .FirstOrDefaultAsync(r => r.Status == RatingStatus.Pending && r.FromUserId == userId);
            if (pendingRating == null)
            {
                throw new NotFoundException(nameof(RatingToProduct), "Pending");
            }
            return this.mapper.Map<RatingToProductDto>(pendingRating);
        }
    }

}
