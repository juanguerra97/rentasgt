using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace rentasgt.Infrastructure.Identity
{
    public class IdentityClaimsProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityClaimsProfileService(UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IUserClaimsPrincipalFactory<AppUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            // note: to dynamically add roles (ie. for users other than consumers - simply look them up by sub id
            claims.AddRange(roles.Select(r => new Claim(JwtClaimTypes.Role, r))); // need this for role-based authorization - https://stackoverflow.com/questions/40844310/role-based-authorization-with-identityserver4

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null && user.ProfileStatus != UserProfileStatus.Inactive;
        }
    }
}
