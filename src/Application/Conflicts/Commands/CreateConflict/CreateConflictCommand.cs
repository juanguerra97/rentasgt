using AutoMapper;
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

namespace rentasgt.Application.Conflicts.Commands.CreateConflict
{
    public class CreateConflictCommand : IRequest<long>
    {
        public long RentId { get; set; }
        public string Description { get; set; }

    }

    public class CreateConflictCommandHandler : IRequestHandler<CreateConflictCommand, long>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;
        private readonly IMapper mapper;

        public CreateConflictCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTime = dateTime;
            this.mapper = mapper;
        }

        public async Task<long> Handle(CreateConflictCommand request, CancellationToken cancellationToken)
        {

            var userId = this.currentUserService.UserId;
            var rentEntity = await this.context.Rents
                .Include(r => r.Request.Requestor)
                .Include(r => r.Request.Product.Owner)
                .FirstOrDefaultAsync(r => r.RequestId == request.RentId);
                
            if (rentEntity.Status != RentStatus.ProductDelivered
                && rentEntity.Status != RentStatus.ReturnDelayed)
            {
                throw new InvalidStateException("Esta renta ya no puede ser reportada");
            }

            if (rentEntity.Request.RequestorId != userId 
                && rentEntity.Request.Product.Owner.Id != userId)
            {
                throw new OperationForbidenException("No puedes reportar esta renta");
            }

            var newConflict = new Conflict
            {
                Description = request.Description,
                ComplainantId = userId,
                ConflictDate = this.dateTime.Now,
                Rent = rentEntity,
                Status = ConflictStatus.Pending
            };
            rentEntity.Status = RentStatus.Conflict;

            await this.context.Conflicts.AddAsync(newConflict);
            await this.context.SaveChangesAsync(cancellationToken);

            return newConflict.Id;

        }
    }

}
