using FluentValidation;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {

        private readonly IApplicationDbContext _context;

        public CreateCategoryCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(Category.MAX_NAME_LENGTH).WithMessage($"El nombre no debe sobrepasar los {Category.MAX_NAME_LENGTH} caracteres")
                .MustAsync(BeUniqueName).WithMessage("El nombre especificado ya existe");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("La descripcion es obligatoria")
                .MaximumLength(Category.MAX_DESCRIPTION_LENGTH).WithMessage($"La descripcion no debe sobrepasar los {Category.MAX_DESCRIPTION_LENGTH} caracteres");

        }

        public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AllAsync(c => c.Name != name);
        }

    }
}
