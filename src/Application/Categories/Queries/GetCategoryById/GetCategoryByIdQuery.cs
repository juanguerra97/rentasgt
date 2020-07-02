using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Categories.Queries.GetCategories;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<CategoryDto>
    {

        public long Id { get; set; }

        public GetCategoryByIdQuery(long id)
        {
            Id = id;
        }

    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {

        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {

            Category entity = await this.context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            return this.mapper.Map<Category, CategoryDto>(entity);

        }
    }

}
