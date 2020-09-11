using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace rentasgt.Infrastructure.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<AppUser> userManager;

        public CustomProfileService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await this.userManager.GetUserAsync(context.Subject);
            context.IssuedClaims.AddRange(new Claim[]
            {
                new Claim("userId", user.Id),
                new Claim("profileStatus", "" + (int)user.ProfileStatus),
                new Claim("email", user.Email),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            });
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await this.userManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null) && user.ProfileStatus != UserProfileStatus.Inactive;
        }
    }
}
