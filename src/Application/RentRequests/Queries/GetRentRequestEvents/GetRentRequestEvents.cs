using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.RentRequests.Queries.GetRentRequests;
using rentasgt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RentRequests.Queries.GetRentRequestEvents
{
    public class GetRentRequestEventsQuery : IRequest<List<RequestEventDto>>
    {

        public long RentRequestId { get; set; }

    }

    public class GetRentRequestEventsQueryHadler : IRequestHandler<GetRentRequestEventsQuery, List<RequestEventDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetRentRequestEventsQueryHadler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<List<RequestEventDto>> Handle(GetRentRequestEventsQuery request, CancellationToken cancellationToken)
        {

            var requestEntity = await this.context.RentRequests
                .Include(rq => rq.Product).ThenInclude(p => p.Owner)
                .Include(rq => rq.Events)
                .FirstOrDefaultAsync(rq => rq.Id == request.RentRequestId);

            if (requestEntity == null)
            {
                throw new NotFoundException(nameof(RentRequest), request.RentRequestId);
            }

            var userId = this.currentUserService.UserId;
            if (userId != requestEntity.RequestorId && userId != requestEntity.Product.Owner.Id)
            {
                throw new OperationForbidenException("No puedes acceder a este historial");
            }

            return this.mapper.Map<List<RequestEventDto>>(requestEntity.Events.OrderByDescending(e => e.EventDate));
        }
    }

}
