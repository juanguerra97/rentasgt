using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Application.Products.Queries.GetProducts;
using rentasgt.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Products.Queries.GetProductsOfOwner
{
    public class GetProductsOfOwner : IRequest<PaginatedListResponse<ProductDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetproductsOfOwnerHandler : IRequestHandler<GetProductsOfOwner, PaginatedListResponse<ProductDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetproductsOfOwnerHandler(IApplicationDbContext context, 
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<PaginatedListResponse<ProductDto>> Handle(GetProductsOfOwner request, CancellationToken cancellationToken)
        {
            var ownerId = this.currentUserService.UserId;
            var products = this.context.Products
                .Include(p => p.Owner)
                .Include(p => p.Pictures).ThenInclude(pp => pp.Picture)
                .Include(p => p.Categories).ThenInclude(pc => pc.Category)
                .Where(p => p.Status != ProductStatus.Inactive && p.Owner.Id == ownerId)
                .OrderBy(p => p.Id);
            return PaginatedListResponse<ProductDto>.ToPaginatedListResponse(products.ProjectTo<ProductDto>(this.mapper.ConfigurationProvider), request.PageNumber, request.PageSize);
        }
    }


}
