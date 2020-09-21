using FluentValidation;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.Conflicts.Commands.CreateConflict
{
    public class CreateConflictCommandValidator : AbstractValidator<CreateConflictCommand>
    {

        public CreateConflictCommandValidator()
        {
            RuleFor(c => c.Description)
                .MaximumLength(Conflict.MAX_DESCRIPTION_LENGTH).WithMessage($"La descripción solo puede tener un máximo de {Conflict.MAX_DESCRIPTION_LENGTH} caracteres");
        }

    }
}
