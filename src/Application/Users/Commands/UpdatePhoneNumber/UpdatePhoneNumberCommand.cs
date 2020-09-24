using MediatR;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace rentasgt.Application.Users.Commands.UpdatePhoneNumber
{
    public class UpdatePhoneNumberCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }

    }

    public class UpdatePhoneNumberCommandHandler : IRequestHandler<UpdatePhoneNumberCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IPhoneVerifyService phoneVerifyService;

        public UpdatePhoneNumberCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IPhoneVerifyService phoneVerifyService)
        {
            this.currentUserService = currentUserService;
            this.phoneVerifyService = phoneVerifyService;
            this.context = context;
            
        }

        public async Task<bool> Handle(UpdatePhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var userId = this.currentUserService.UserId;
            if (userId != request.UserId)
            {
                throw new OperationForbidenException();
            }
            var userEntity = await this.context.AppUsers.FirstOrDefaultAsync(u => u.Id == userId);
            
            if (userEntity == null)
            {
                throw new NotFoundException("User", request.UserId);
            }

            userEntity.PhoneNumber = $"{request.PhoneNumber}";
            var status = await this.phoneVerifyService.SendVerificationCode($"+502{userEntity.PhoneNumber}");
            userEntity.PhoneNumberConfirmed = status == "approved";
            
            // TODO: change profile status
            
            await this.context.SaveChangesAsync(cancellationToken);

            return userEntity.PhoneNumberConfirmed;
        }

    }

}