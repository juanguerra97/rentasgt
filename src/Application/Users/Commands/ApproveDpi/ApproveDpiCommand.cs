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

namespace rentasgt.Application.Users.Commands.ApproveDpi
{
    public class ApproveDpiCommand : IRequest
    {
        public string UserId { get; set; }
    }

    public class ApproveDpiCommandHandler : IRequestHandler<ApproveDpiCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;

        public ApproveDpiCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTime = dateTime;
        }

        public async Task<Unit> Handle(ApproveDpiCommand request, CancellationToken cancellationToken)
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
            if (user.ValidatedDpi)
            {
                throw new InvalidStateException("El DPI ya fué validado");
            }
            user.ValidatedDpi = true;
            user.ProfileEvents.Add(new UserProfileEvent
            {
                EventType = UserProfileEventType.DpiAccepted,
                Message = "DPI validado",
                EventDate = this.dateTime.Now,
                UserEvent = await this.context.AppUsers.FirstOrDefaultAsync(u => u.Id == this.currentUserService.UserId)
            });
            if (user.ValidatedAddress)
            {
                user.ProfileStatus = UserProfileStatus.Active;
            }
            await this.context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}
