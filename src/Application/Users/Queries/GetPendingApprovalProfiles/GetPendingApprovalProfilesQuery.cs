using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Application.Users.Queries.GetUserProfile;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Users.Queries.GetPendingApprovalProfiles
{
    public class GetPendingApprovalProfilesQuery : IRequest<PaginatedListResponse<UserProfileDto>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }

    public class GetPendingApprovalProfilesQueryHandler : IRequestHandler<GetPendingApprovalProfilesQuery, PaginatedListResponse<UserProfileDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetPendingApprovalProfilesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<PaginatedListResponse<UserProfileDto>> Handle(GetPendingApprovalProfilesQuery request, CancellationToken cancellationToken)
        {
            var userId = this.currentUserService.UserId;
            var users = this.context.AppUsers
                .Where(u => u.Id != userId &&  u.ProfileStatus == UserProfileStatus.WaitingForApproval)
                .OrderBy(u => u.Id);
            return PaginatedListResponse<UserProfileDto>
                .ToPaginatedListResponse(users.ProjectTo<UserProfileDto>(this.mapper.ConfigurationProvider), 
                request.PageNumber, request.PageSize);
        }
    }

}
