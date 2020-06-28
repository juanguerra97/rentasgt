using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<List<CategoryDto>>
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetCategoriesQuery(int pageSize = 15, int pageNumber = 1)
        {
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
        }

    }

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {

        private readonly IApplicationDbContext context;
        private readonly Mapper mapper;

        public GetCategoriesQueryHandler(IApplicationDbContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return mapper.Map<List<Category>, List<CategoryDto>>(
                await this.context.Categories.Skip(request.PageSize * (request.PageNumber - 1))
                    .Take(request.PageSize).OrderBy(c => c.Id).ToListAsync());
        }
    }

}
