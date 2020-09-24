using FluentValidation;
using rentasgt.Application.Common.Interfaces;

namespace rentasgt.Application.Users.Commands.UpdatePhoneNumber
{
    public class UpdatePhoneNumberCommandValidator : AbstractValidator<UpdatePhoneNumberCommand>
    {
        private readonly IPhoneVerifyService phoneVerifyService;

        public UpdatePhoneNumberCommandValidator(IPhoneVerifyService phoneVerifyService)
        {

            this.phoneVerifyService = phoneVerifyService;

            RuleFor(c => c.PhoneNumber)
                .MustAsync((number, cancellationToken) => this.phoneVerifyService.IsValidPhoneNumber(number))
                    .WithMessage("El número no es válido");
            
        }

        

    }
}
