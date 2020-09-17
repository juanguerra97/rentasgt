using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace rentasgt.Infrastructure.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> claimsFactory;

        public CustomProfileService(UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IUserClaimsPrincipalFactory<AppUser> claimsFactory)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.claimsFactory = claimsFactory;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await this.userManager.GetUserAsync(context.Subject);
            var roles = await this.userManager.GetRolesAsync(user);

            context.IssuedClaims.AddRange(new Claim[]
            {
                new Claim("userId", user.Id),
                new Claim("profileStatus", "" + (int)user.ProfileStatus),
                new Claim("email", user.Email),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            });
            context.IssuedClaims.AddRange(roles.Select(r => new Claim(JwtClaimTypes.Role, r)));
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await this.userManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null) && user.ProfileStatus != UserProfileStatus.Inactive;
        }
    }
}
