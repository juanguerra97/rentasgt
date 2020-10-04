using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Application.RatingToProducts.Queries.GetPendingRatingToProduct;
using rentasgt.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RatingToProducts.Queries.GetRatingsToProduct
{
    public class GetRatingsToProductQuery : IRequest<PaginatedListResponse<RatingToProductDto>>
    {
        public long ProductId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetRatingsToProductQueryHandler : IRequestHandler<GetRatingsToProductQuery, PaginatedListResponse<RatingToProductDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetRatingsToProductQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PaginatedListResponse<RatingToProductDto>> Handle(GetRatingsToProductQuery request, CancellationToken cancellationToken)
        {
            var ratings = this.context.RatingToProducts
                .Include(r => r.Product.Owner)
                .Include(r => r.Product.Pictures).ThenInclude(p => p.Picture)
                .Include(r => r.FromUser)
                .Where(r => r.Status == RatingStatus.Rated && r.Product.Id == request.ProductId)
                .ProjectTo<RatingToProductDto>(this.mapper.ConfigurationProvider)
                .OrderByDescending(r => r.RatingDate);

            return PaginatedListResponse<RatingToProductDto>
                .ToPaginatedListResponse(ratings, request.PageNumber, request.PageSize);
        }
    }

}
