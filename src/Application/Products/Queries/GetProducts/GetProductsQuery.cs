using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<List<ProductDto>>
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetProductsQuery(int pageSize = 15, int pageNumber = 1)
        {
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
        }

    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {

            return await this.context.Products
                .Include(p => p.Owner)
                .Include(p => p.Pictures).ThenInclude(pp => pp.Picture)
                .Include(p => p.Costs)
                .Include(p => p.Categories).ThenInclude(pc => pc.Category)
                .Where(p => p.Status != ProductStatus.Inactive && p.Status != ProductStatus.Incomplete)
                .OrderBy(p => p.Id)
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ProjectTo<ProductDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }

}
