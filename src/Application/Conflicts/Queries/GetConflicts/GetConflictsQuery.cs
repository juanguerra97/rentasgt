using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Conflicts.Queries.GetConflicts
{
    public class GetConflictsQuery : IRequest<PaginatedListResponse<ConflictDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public ConflictStatus? ConflictStatus { get; set; }
        public string ModeratorId { get; set; }
    }

    public class GetConflictsQueryHandler : IRequestHandler<GetConflictsQuery, PaginatedListResponse<ConflictDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetConflictsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PaginatedListResponse<ConflictDto>> Handle(GetConflictsQuery request, CancellationToken cancellationToken)
        {

            var conflicts = this.context.Conflicts
                .Include(c => c.ConflictRecords)
                .Include(c => c.Complainant)
                .Include(c => c.Moderator)
                .Include(c => c.Rent)
                .ThenInclude(r => r.Request)
                .ThenInclude(rq => rq.Product)
                .ThenInclude(p => p.Owner)
                .Include(c => c.Rent.Request.Requestor)
                .Where(c => (request.ModeratorId == null || c.ModeratorId == request.ModeratorId) 
                    && (request.ConflictStatus == null || c.Status == request.ConflictStatus));

            return PaginatedListResponse<ConflictDto>
                .ToPaginatedListResponse(conflicts
                    .ProjectTo<ConflictDto>(this.mapper.ConfigurationProvider)
                    .OrderByDescending(c => c.ConflictDate), 
                    request.PageNumber, request.PageSize);
        }
    }

}
