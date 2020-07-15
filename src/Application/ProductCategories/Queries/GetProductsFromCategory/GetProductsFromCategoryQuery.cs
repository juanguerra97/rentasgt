using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Products.Queries.GetProducts;
using rentasgt.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.ProductCategories.Queries.GetProductsFromCategory
{
    public class GetProductsFromCategoryQuery : IRequest<List<ProductDto>>
    {

        public long CategoryId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }

    public class GetProductsFromCategoryQueryHandler : IRequestHandler<GetProductsFromCategoryQuery, List<ProductDto>>
    {

        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetProductsFromCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetProductsFromCategoryQuery request, CancellationToken cancellationToken)
        {
            return await this.context.ProductCategories.Include(pc => pc.Product)
                .Where(pc => pc.CategoryId == request.CategoryId)
                .Select(pc => pc.Product)
                .Where(p => p.Status != ProductStatus.Inactive && p.Status != ProductStatus.Incomplete)
                .OrderBy(p => p.Id)
                .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                .ProjectTo<ProductDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();
        }

    }

}
