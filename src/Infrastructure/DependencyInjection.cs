using rentasgt.Application.Common.Interfaces;
using rentasgt.Infrastructure.Files;
using rentasgt.Infrastructure.Identity;
using rentasgt.Infrastructure.Persistence;
using rentasgt.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using rentasgt.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityServer4.Services;
using IdentityModel;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace rentasgt.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("rentasgtDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddDefaultIdentity<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<AppUser, ApplicationDbContext>(configure =>
                {
                    //configure.IdentityResources.Add(new IdentityServer4.Models.IdentityResource(
                    //    "roles", "Roles", new string[] { JwtClaimTypes.Role }));
                    //foreach (var c in configure.Clients)
                    //{
                    //    if (c.ClientId == "rentasgt.WebUI")
                    //    {
                    //        c.AllowedScopes.Add("roles");
                    //    }
                    //}
                });

            services.AddTransient<IProfileService, IdentityClaimsProfileService>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<ILocation, LocationService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();            

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerJwt()
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("OnlyAdmin",
                    options => {
                        options.RequireAssertion(context => {
                            return context.User.HasClaim(c => (c.Type == ClaimTypes.Role || c.Type == JwtClaimTypes.Role) && c.Value == "Admin");
                        });
                        });
            });

            return services;
        }
    }
}
