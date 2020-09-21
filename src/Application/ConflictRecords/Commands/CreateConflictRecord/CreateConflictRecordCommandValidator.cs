using FluentValidation;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.ConflictRecords.Commands.CreateConflictRecord
{
    public class CreateConflictRecordCommandValidator : AbstractValidator<CreateConflictRecordCommand>
    {

        public CreateConflictRecordCommandValidator()
        {
            RuleFor(c => c.Description)
                .MaximumLength(ConflictRecord.MAX_DESCRIPTION_LENGTH).WithMessage($"La descripción solo puede tener un máximo de {ConflictRecord.MAX_DESCRIPTION_LENGTH} caracteres");
        }

    }
}
