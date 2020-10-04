using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Application.RentRequests.Queries.GetRentRequests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RentRequests.Queries.GetRentRequestsOfOwner
{
    public class GetRentRequestsOfOwnerQuery : IRequest<PaginatedListResponse<RentRequestDto>>
    {

        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }

    public class GetRentRequestsOfOwnerQueryHandler : IRequestHandler<GetRentRequestsOfOwnerQuery, PaginatedListResponse<RentRequestDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetRentRequestsOfOwnerQueryHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<PaginatedListResponse<RentRequestDto>> Handle(GetRentRequestsOfOwnerQuery request, CancellationToken cancellationToken)
        {
            var rentRequests = this.context.RentRequests
                 .Include(rq => rq.Requestor)
                 .ThenInclude(r => r.ProfilePicture)
                 .ThenInclude(p => p.Picture)
                 .Include(rq => rq.Product)
                 .ThenInclude(rq => rq.Owner)
                 .ThenInclude(o => o.ProfilePicture)
                 .ThenInclude(p => p.Picture)
                 .Include(rq => rq.Product.Pictures)
                 .Where(rq => rq.Product.Owner.Id == currentUserService.UserId)
                 .ProjectTo<RentRequestDto>(mapper.ConfigurationProvider)
                 .OrderByDescending(rq => rq.RequestDate);
            return PaginatedListResponse<RentRequestDto>
                .ToPaginatedListResponse(rentRequests, request.PageNumber, request.PageSize);
        }
    }

}
