using MediatR;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Categories.Commands.UpdateCategory
{
    public partial class UpdateCategoryCommand : IRequest
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {

        private readonly IApplicationDbContext context;

        public UpdateCategoryCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.Categories.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }
            
            if (request.Name != null)
            {
                entity.Name = request.Name;
            }

            if (request.Description != null)
            {
                entity.Description = request.Description;
            }

            if (request.Name != null || request.Description != null)
            {
                await this.context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }

}
