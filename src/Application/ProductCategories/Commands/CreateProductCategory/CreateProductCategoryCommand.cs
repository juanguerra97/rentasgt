using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.ProductCategories.Commands.CreateProductCategory
{
    public class CreateProductCategoryCommand : IRequest
    {

        public long ProductId { get; set; }
        public long CategoryId { get; set; }

    }

    public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand>
    {
        private readonly IApplicationDbContext context;

        public CreateProductCategoryCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
        {

            var entity = await this.context.ProductCategories.SingleOrDefaultAsync(pc => pc.ProductId == request.ProductId && pc.CategoryId == request.CategoryId);
            if (entity != null)
            {
                throw new DuplicateDataException("Datos duplicados");
            }

            var productEntity = await this.context.Products.SingleOrDefaultAsync(p => p.Id == request.ProductId);
            if (productEntity == null)
            {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            var categoryEntity = await this.context.Categories.SingleOrDefaultAsync(c => c.Id == request.CategoryId);
            if (categoryEntity == null)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            await this.context.ProductCategories.AddAsync(new ProductCategory
            {
                Product = productEntity,
                Category = categoryEntity
            });

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
