using MediatR;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest
    {

        public long Id { get; set; }

    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IApplicationDbContext context;

        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = this.context.Products.FirstOrDefault(p => p.Id == request.Id);
            
            if (entity == null)
            {
                throw new NotFoundException(nameof(Products), request.Id);
            }

            //this.context.Products.Remove(entity);
            entity.Status = ProductStatus.Inactive;

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }

}
