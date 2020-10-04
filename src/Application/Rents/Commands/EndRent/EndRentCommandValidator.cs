using FluentValidation;

namespace rentasgt.Application.Rents.Commands.EndRent
{
    public class EndRentCommandValidator : AbstractValidator<EndRentCommand>
    {

        public EndRentCommandValidator()
        {
            RuleFor(c => c.RatingValue)
                .InclusiveBetween(0, 5).WithMessage("Solo se permiten valores entre {1} y {2}");
        }

    }
}
