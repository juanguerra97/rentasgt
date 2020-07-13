using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest
    {

        public long Id { get; set; }
        public ProductStatus? Status { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? OtherNames { get; set; }
        public Ubicacion? Location { get; set; }

    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {

        private readonly IApplicationDbContext context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            var entity = await this.context.Products.Where(p => p.Id == request.Id)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            bool fieldUpdated = false;

            if (request.Status != null)
            {
                UpdateProductStatus(entity, (ProductStatus) request.Status);
                fieldUpdated = true;
            }

            if (request.Name != null)
            {
                entity.Name = request.Name;
                fieldUpdated = true;
            }

            if (request.Description != null)
            {
                entity.Description = request.Description;
                fieldUpdated = true;
            }

            if (request.OtherNames != null)
            {
                entity.OtherNames = request.OtherNames;
                fieldUpdated = true;
            }

            if(request.Location != null)
            {
                entity.Location = request.Location;
                fieldUpdated = true;
            }

            if (fieldUpdated)
            {
                await this.context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }

        private void UpdateProductStatus(Product product, ProductStatus newProductStatus)
        {
            if (newProductStatus == ProductStatus.Available || newProductStatus == ProductStatus.NotAvailable)
            {
                product.Status = newProductStatus;
            }
            else
            {
                throw new Exception("El nuevo estado es invalido");
            }
        }

    }

}
