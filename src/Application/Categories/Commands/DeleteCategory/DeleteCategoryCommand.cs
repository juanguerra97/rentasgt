using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Categories.Commands.DeleteCategory
{
    public partial class DeleteCategoryCommand : IRequest
    {
        public long Id { get; set; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {

        private readonly IApplicationDbContext context;

        public DeleteCategoryCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.Categories
                .Where(c => c.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            this.context.Categories.Remove(entity);

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
