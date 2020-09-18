using Application.Rents.Queries.GetRentsOfRequestor;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Rents.Queries.GetRentsOfOwner
{
    public class GetRentsOfOwnerQuery : IRequest<PaginatedListResponse<RentRequestRentDto>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class GetRentsOfOwnerQueryHandler : IRequestHandler<GetRentsOfOwnerQuery, PaginatedListResponse<RentRequestRentDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetRentsOfOwnerQueryHandler(IApplicationDbContext context, 
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<PaginatedListResponse<RentRequestRentDto>> Handle(GetRentsOfOwnerQuery request, CancellationToken cancellationToken)
        {
            var rents = this.context.RentRequests
                .Include(rq => rq.Requestor)
                .Include(rq => rq.Product).ThenInclude(p => p.Owner)
                .Include(rq => rq.Rent)
                .Where(rq => rq.Rent != null && rq.Product.Owner.Id == this.currentUserService.UserId);

            return PaginatedListResponse<RentRequestRentDto>
                .ToPaginatedListResponse(rents.ProjectTo<RentRequestRentDto>(this.mapper.ConfigurationProvider)
                    .OrderByDescending(r => r.Id),
                    request.PageNumber, request.PageSize);
        }
    }

}
