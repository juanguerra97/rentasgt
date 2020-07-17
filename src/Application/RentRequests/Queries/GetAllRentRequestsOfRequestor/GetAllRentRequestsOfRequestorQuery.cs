using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.RentRequests.Queries.GetRentRequests;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RentRequests.Queries.GetAllRentRequestsOfRequestor
{
    public class GetAllRentRequestsOfRequestorQuery : IRequest<List<RentRequestDto>>
    {

        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }

    public class GetAllRentRequestsOfRequestorQueryHandler : IRequestHandler<GetAllRentRequestsOfRequestorQuery, List<RentRequestDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetAllRentRequestsOfRequestorQueryHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<List<RentRequestDto>> Handle(GetAllRentRequestsOfRequestorQuery request, CancellationToken cancellationToken)
        {
            return await this.context.RentRequests
                .Include(rq => rq.Product)
                .Where(rq => rq.RequestorId == currentUserService.UserId)
                .OrderByDescending(rq => rq.RequestDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<RentRequestDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }


}
