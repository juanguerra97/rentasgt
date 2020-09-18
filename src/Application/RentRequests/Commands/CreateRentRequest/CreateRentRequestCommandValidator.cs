using FluentValidation;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.RentRequests.Commands.CreateRentRequest
{
    public class CreateRentRequestCommandValidator : AbstractValidator<CreateRentRequestCommand>
    {

        public CreateRentRequestCommandValidator()
        {

            RuleFor(c => c.StartDate.Date)
                .LessThanOrEqualTo(c => c.EndDate.Date).WithMessage("La fecha de inicio debe ser menor a la fecha fin");

            RuleFor(c => c.Place)
                .NotEmpty()
                .MaximumLength(RentRequest.MAX_PLACE_LENGTH).WithMessage($"El lugar no debe sobrepasar los {RentRequest.MAX_PLACE_LENGTH} caracteres")
                .When(c => c.Place != null);
        }

    }
}
