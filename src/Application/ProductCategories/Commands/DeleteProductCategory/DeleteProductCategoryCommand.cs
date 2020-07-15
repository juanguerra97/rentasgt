using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.ProductCategories.Commands.DeleteProductCategory
{
    public class DeleteProductCategoryCommand : IRequest
    {

        public long ProductId { get; set; }
        public long CategoryId { get; set; }

    }

    public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand>
    {
        private readonly IApplicationDbContext context;

        public DeleteProductCategoryCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {

            var entity = await this.context.ProductCategories
                .Where(pc => pc.ProductId == request.ProductId && pc.CategoryId == request.CategoryId)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductCategory), $"{request.ProductId}/{request.CategoryId}");
            }

            this.context.ProductCategories.Remove(entity);
            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
