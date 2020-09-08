using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Rents.Queries.GetRentsOfRequestor;
using rentasgt.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Rents.Queries.GetRentById
{
    public class GetRentByIdQuery : IRequest<RentDto>
    {
        public long RentId { get; set; }
    }

    public class GetRentByIdQueryHandler : IRequestHandler<GetRentByIdQuery, RentDto>
    {

        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetRentByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<RentDto> Handle(GetRentByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = this.currentUserService.UserId;
            var rentEntity = await this.context.Rents
                .Include(r => r.Request)
                    .ThenInclude(rq => rq.Events)
                .Include(r => r.Request.Product.Owner)
                .Include(r => r.Request.Requestor)
                .FirstOrDefaultAsync(r => r.RequestId == request.RentId 
                    && (r.Request.RequestorId == userId || r.Request.Product.Owner.Id == userId));

            if (rentEntity == null)
            {
                throw new NotFoundException(nameof(Rent), request.RentId);
            }
            return this.mapper.Map<RentDto>(rentEntity);
        }
    }

}
