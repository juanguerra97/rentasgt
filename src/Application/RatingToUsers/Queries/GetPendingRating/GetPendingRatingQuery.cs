using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.RatingToUsers.Queries.GetPendingRating
{
    public class GetPendingRatingQuery : IRequest<RatingToUserDto>
    {
    }

    public class GetPendingRatingQueryHandler : IRequestHandler<GetPendingRatingQuery, RatingToUserDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetPendingRatingQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<RatingToUserDto> Handle(GetPendingRatingQuery request, CancellationToken cancellationToken)
        {
            var userId = this.currentUserService.UserId;
            var pendingRating = await this.context.RatingToUsers
                .FirstOrDefaultAsync(r => r.FromUserId == userId);

            if (pendingRating == null)
            {
                throw new NotFoundException(nameof(RatingToUser), "");
            }

            return this.mapper.Map<RatingToUserDto>(pendingRating);
        }
    }

}