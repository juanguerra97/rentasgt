using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Application.RentRequests.Queries.GetRentRequests;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RentRequests.Queries.GetAllRentRequestsOfRequestor
{
    public class GetAllRentRequestsOfRequestorQuery : IRequest<PaginatedListResponse<RentRequestDto>>
    {

        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }

    public class GetAllRentRequestsOfRequestorQueryHandler : IRequestHandler<GetAllRentRequestsOfRequestorQuery, PaginatedListResponse<RentRequestDto>>
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

        public async Task<PaginatedListResponse<RentRequestDto>> Handle(GetAllRentRequestsOfRequestorQuery request, CancellationToken cancellationToken)
        {
            var rentRequests = this.context.RentRequests
                .Include(rq => rq.Requestor)
                .Include(rq => rq.Product)
                .ThenInclude(rq => rq.Owner)
                .Include(rq => rq.Product.Pictures)
                .Where(rq => rq.RequestorId == currentUserService.UserId)
                .ProjectTo<RentRequestDto>(mapper.ConfigurationProvider)
                .OrderByDescending(rq => rq.RequestDate);
            return  PaginatedListResponse<RentRequestDto>
                .ToPaginatedListResponse(rentRequests, request.PageNumber, request.PageSize);
        }
    }


}
