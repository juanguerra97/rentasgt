using FluentValidation;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {

        private readonly IApplicationDbContext context;

        public UpdateCategoryCommandValidator(IApplicationDbContext context)
        {
            this.context = context;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(Category.MAX_NAME_LENGTH)
                    .WithMessage($"El nombre no debe sobrepasar los {Category.MAX_NAME_LENGTH} caracteres")
                .MustAsync(BeUniqueName).WithMessage("El nombre especificado ya existe")
                .When(c => c.Name != null);

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("La descripcion es obligatoria")
                .MaximumLength(Category.MAX_DESCRIPTION_LENGTH)
                    .WithMessage($"La descripcion no debe sobrepasar los {Category.MAX_DESCRIPTION_LENGTH} caracteres")
                .When(c => c.Description != null);
        }

        public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return await this.context.Categories
                .AllAsync(c => c.Name != name);
        }

    }
}
