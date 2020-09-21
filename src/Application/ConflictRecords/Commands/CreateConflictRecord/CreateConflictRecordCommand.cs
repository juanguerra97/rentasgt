using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.ConflictRecords.Commands.CreateConflictRecord
{
    public class CreateConflictRecordCommand : IRequest<long>
    {
        public long ConflictId { get; set; }
        public string Description { get; set; }
    }

    public class CreateConflictRecordCommandHandler : IRequestHandler<CreateConflictRecordCommand, long>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;

        public CreateConflictRecordCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTime = dateTime;
        }

        public async Task<long> Handle(CreateConflictRecordCommand request, CancellationToken cancellationToken)
        {

            var conflictEntity = await this.context.Conflicts
                .Include(c => c.ConflictRecords)
                .FirstOrDefaultAsync(c => c.Id == request.ConflictId);

            if (conflictEntity == null)
            {
                throw new NotFoundException(nameof(ConflictRecord), request.ConflictId);
            }

            var userId = this.currentUserService.UserId;
            if (userId != conflictEntity.ModeratorId)
            {
                throw new OperationForbidenException();
            }

            var newRecord = new ConflictRecord
            {
                Conflict = conflictEntity,
                Description = request.Description,
                RecordDate = this.dateTime.Now
            };

            conflictEntity.ConflictRecords.Add(newRecord);
            await this.context.SaveChangesAsync(cancellationToken);

            return newRecord.Id;
        }
    }

}
