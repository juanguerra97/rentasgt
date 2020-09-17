using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Users.Commands.ApproveAddress
{
    public class ApproveAddressCommand : IRequest
    {
        public string UserId { get; set; }
    }

    public class ApprovedAddressCommandHandler : IRequestHandler<ApproveAddressCommand>
    {

        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;

        public ApprovedAddressCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTime = dateTime;
        }

        public async Task<Unit> Handle(ApproveAddressCommand request, CancellationToken cancellationToken)
        {
            var user = await this.context.AppUsers
                .FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                throw new NotFoundException("User", request.UserId);
            }
            if (user.Id == currentUserService.UserId)
            {
                throw new OperationForbidenException();
            }
            if (user.ValidatedAddress)
            {
                throw new InvalidStateException("Dirección ya fué validada");
            }
            user.ValidatedAddress = true;
            user.ProfileEvents.Add(new UserProfileEvent
            {
                EventType = UserProfileEventType.AddressPictureAccepted,
                Message = "Dirección validada",
                EventDate = this.dateTime.Now,
                UserEvent = await this.context.AppUsers.FirstOrDefaultAsync(u => u.Id == this.currentUserService.UserId)
            });
            if (user.ValidatedDpi)
            {
                user.ProfileStatus = UserProfileStatus.Active;
            }
            await this.context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
