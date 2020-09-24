using FluentValidation;
using rentasgt.Application.Common.Interfaces;

namespace rentasgt.Application.Users.Commands.SendPhoneNumberVerificationCode
{
    public class SendPhoneNumberVerificationCodeValidator : AbstractValidator<SendPhoneNumberVerificationCode>
    {

        public SendPhoneNumberVerificationCodeValidator(IPhoneVerifyService phoneVerifyService)
        {
            RuleFor(c => c.PhoneNumber)
               .MustAsync((number, cancellationToken) => phoneVerifyService.IsValidPhoneNumber(number))
                   .WithMessage("El número no es válido");
        }

    }
}
