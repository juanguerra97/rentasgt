using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Users.Queries.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<UserProfileDto>
    {
    }

    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public GetUserProfileQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await this.context.AppUsers
                .Include(u => u.UserPicture.Picture)
                .Include(u => u.ProfilePicture.Picture)
                .Include(u => u.DpiPicture.Picture)
                .Include(u => u.AddressPicture.Picture)
                .FirstOrDefaultAsync(u => u.Id == this.currentUserService.UserId);

            if (user == null) // this should never be true, I'll let it here just in case
            {
                throw new NotFoundException("User", this.currentUserService.UserId); ;
            }

            return this.mapper.Map<UserProfileDto>(user);
        }
    }

}
