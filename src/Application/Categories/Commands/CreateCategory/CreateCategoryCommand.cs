using AutoMapper;
using MediatR;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Categories.Commands.CreateCategory
{
    public partial class CreateCategoryCommand : IRequest<long>
    {

        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, long>
    {

        private readonly IApplicationDbContext context;
        private readonly Mapper mapper;

        public CreateCategoryCommandHandler(IApplicationDbContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<long> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {

            var newCategory = mapper.Map<CreateCategoryCommand, Category>(request);

            await this.context.Categories.AddAsync(newCategory);
            await this.context.SaveChangesAsync(cancellationToken);

            return newCategory.Id;

        }
    }

}
