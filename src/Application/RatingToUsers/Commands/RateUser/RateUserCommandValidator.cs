using FluentValidation;

namespace rentasgt.Application.RatingToUsers.Commands.RateUser
{
    public class RateUserCommandValidator : AbstractValidator<RateUserCommand>
    {

        public RateUserCommandValidator()
        {
            RuleFor(c => c.RatingValue)
                .InclusiveBetween(0, 5).WithMessage("Solo se permiten valores entre {1} y {2}");
        }

    }

}
