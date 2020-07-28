using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<PaginatedListResponse<CategoryDto>>
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetCategoriesQuery(int pageSize = 15, int pageNumber = 1)
        {
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
        }

    }

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PaginatedListResponse<CategoryDto>>
    {

        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PaginatedListResponse<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return PaginatedListResponse<CategoryDto>.ToPaginatedListResponse(this.context.Categories.OrderBy(c => c.Id).ProjectTo<CategoryDto>(this.mapper.ConfigurationProvider), request.PageNumber, request.PageSize);
        }
    }

}
