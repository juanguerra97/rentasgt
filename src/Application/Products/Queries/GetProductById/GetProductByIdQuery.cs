using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Products.Queries.GetProducts;
using rentasgt.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {

        public long Id { get; set; }

    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetProductByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await this.context.Products.Include(p => p.Owner)
                .Include(p => p.Pictures).ThenInclude(pp => pp.Picture)
                .Include(p => p.Costs)
                .Include(p => p.Categories).ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }
            return this.mapper.Map<Product, ProductDto>(entity);
        }
    }

}