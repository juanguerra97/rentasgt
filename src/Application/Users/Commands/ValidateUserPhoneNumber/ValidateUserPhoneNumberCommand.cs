using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Users.Commands.ValidateUserPhoneNumber
{
    public class ValidateUserPhoneNumberCommand : IRequest<bool>
    {

        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }

    }

    public class ValidateUserPhoneNumberCommandHandler : IRequestHandler<ValidateUserPhoneNumberCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IPhoneVerifyService phoneVerifyService;

        public ValidateUserPhoneNumberCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IPhoneVerifyService phoneVerifyService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.phoneVerifyService = phoneVerifyService;
        }

        public async Task<bool> Handle(ValidateUserPhoneNumberCommand request, CancellationToken cancellationToken)
        {

            var userId = this.currentUserService.UserId;
            var userEntity = await this.context.AppUsers
                .Include(u => u.DpiPicture.Picture)
                .Include(u => u.UserPicture.Picture)
                .Include(u => u.AddressPicture.Picture)
                .Include(u => u.ProfilePicture.Picture)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (userEntity == null)
            {
                throw new NotFoundException("User", userId);
            }

            if (await this.phoneVerifyService.VerifyCode($"+502{request.PhoneNumber}", request.VerificationCode))
            {
                userEntity.PhoneNumber = request.PhoneNumber;
                userEntity.PhoneNumberConfirmed = true;

                if(userEntity.ProfileStatus == UserProfileStatus.Incomplete
                    && (userEntity.ProfilePicture != null && userEntity.DpiPicture != null
                    && userEntity.UserPicture != null && userEntity.AddressPicture != null
                    && userEntity.EmailConfirmed))
                {
                    userEntity.ProfileStatus = UserProfileStatus.WaitingForApproval;
                }

                await this.context.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }
    }

}
