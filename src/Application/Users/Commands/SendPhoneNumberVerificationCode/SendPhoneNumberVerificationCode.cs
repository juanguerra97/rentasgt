using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Users.Commands.SendPhoneNumberVerificationCode
{
    public class SendPhoneNumberVerificationCode : IRequest
    {
        public string PhoneNumber { get; set; }
    }

    public class SendPhoneNumberVerificationCodeHandler : IRequestHandler<SendPhoneNumberVerificationCode>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IPhoneVerifyService phoneVerifyService;

        public SendPhoneNumberVerificationCodeHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IPhoneVerifyService phoneVerifyService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.phoneVerifyService = phoneVerifyService;
        }

        public async Task<Unit> Handle(SendPhoneNumberVerificationCode request, CancellationToken cancellationToken)
        {
            var userId = this.currentUserService.UserId;
            var userEntity = await this.context.AppUsers
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (userEntity == null) // just in case
            {
                throw new NotFoundException("User", userId);
            }

            var status = await this.phoneVerifyService.SendVerificationCode($"+502{request.PhoneNumber}");
            return Unit.Value;
        }
    }

}
