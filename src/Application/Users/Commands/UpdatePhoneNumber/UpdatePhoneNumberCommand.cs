using System;
using MediatR;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace rentasgt.Application.Users.Commands.UpdatePhoneNumber
{
    public class UpdatePhoneNumberCommand : IRequest
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }

    }

    public class UpdatePhoneNumberCommandHandler : IRequestHandler<UpdatePhoneNumberCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;

        public UpdatePhoneNumberCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            this.currentUserService = currentUserService;
            this.context = context;
            
        }

        public async Task<Unit> Handle(UpdatePhoneNumberCommand request, CancellationToken cancellationToken)
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

            userEntity.PhoneNumber = request.PhoneNumber;
            userEntity.PhoneNumberConfirmed = false;
            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

    }

}