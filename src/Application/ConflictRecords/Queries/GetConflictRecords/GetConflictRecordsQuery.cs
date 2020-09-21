using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.ConflictRecords.Queries.GetConflictRecords
{
    public class GetConflictRecordsQuery : IRequest<List<ConflictRecordDto>>
    {
        public long ConflictId { get; set; }
    }

    public class GetConflictRecorsQueryHandler : IRequestHandler<GetConflictRecordsQuery, List<ConflictRecordDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetConflictRecorsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<List<ConflictRecordDto>> Handle(GetConflictRecordsQuery request, CancellationToken cancellationToken)
        {
            var conflictEntity = await this.context.Conflicts
                .FirstOrDefaultAsync(c => c.Id == request.ConflictId);

            if (conflictEntity == null)
            {
                throw new NotFoundException(nameof(Conflict), request.ConflictId);
            }

            var userId = this.currentUserService.UserId;
            if (userId != conflictEntity.ModeratorId)
            {
                throw new OperationForbidenException();
            }

            var records = this.context.ConflictRecords
                .Where(r => r.ConflictId == conflictEntity.Id)
                .ProjectTo<ConflictRecordDto>(this.mapper.ConfigurationProvider)
                .OrderByDescending(r => r.RecordDate);

            return await records.ToListAsync();
        }
    }

}
