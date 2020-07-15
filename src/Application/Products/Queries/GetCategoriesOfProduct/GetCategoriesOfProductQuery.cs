using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Products.Queries.GetCategoriesOfProduct
{
    public class GetCategoriesOfProductQuery : IRequest<List<CategorySummaryDto>>
    {

        public long ProductId { get; set; }

    }

    public class GetCategoriesOfProductQueryHandler : IRequestHandler<GetCategoriesOfProductQuery, List<CategorySummaryDto>>
    {

        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetCategoriesOfProductQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<CategorySummaryDto>> Handle(GetCategoriesOfProductQuery request, CancellationToken cancellationToken)
        {

            return await context.ProductCategories.Include(pc => pc.Category)
                .Where(pc => pc.ProductId == request.ProductId)
                .Select(pc => pc.Category)
                .OrderBy(c => c.Name)
                .ProjectTo<CategorySummaryDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }


}
