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
using IdentityModel;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Twilio;
using rentasgt.Infrastructure.Models;
using IdentityServer4.Models;
using System.Collections.Generic;
using System;

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
                var serverVersion = new MySqlServerVersion(new Version(5, 7));
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(
                        configuration.GetConnectionString("DefaultConnection"), serverVersion,
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddDefaultIdentity<AppUser>(config => {
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredLength = 1;
                config.SignIn.RequireConfirmedAccount = false;
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                //.AddSigningCredential("CN=rentasgtdev", System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine)
                .AddApiAuthorization<AppUser, ApplicationDbContext>(configure =>
                {
                    configure.Clients.Add(new Client { 
                        ClientId = "rentasgt.MobileApp",
                        RequirePkce = false,
                        AllowedGrantTypes = new List<string> { "authorization_code"},
                        AllowedScopes = new List<string> { "openid", "profile", "rentasgt.WebUIAPI" },
                        RedirectUris = new List<string> { "https://oidcdebugger.com/debug", "com.rentasguatemala://oauth_callback", "rentasgt://callback" },
                        PostLogoutRedirectUris = new List<string> { "com.rentasguatemala://oauth_callback", "rentasgt://callback" },
                        RequireClientSecret = false,
                        RequireConsent = false,
                        AllowAccessTokensViaBrowser = true,
                    });
                    //configure.IdentityResources.Add(new IdentityServer4.Models.IdentityResource(
                    //    "roles", "Roles", new string[] { JwtClaimTypes.Role }));
                    //foreach (var c in configure.Clients)
                    //{
                    //    if (c.ClientId == "rentasgt.WebUI")
                    //    {
                    //        c.AllowedScopes.Add("roles");
                    //    }
                    //}
                })
                .AddProfileService<CustomProfileService>();

            //services.AddTransient<IProfileService, IdentityClaimsProfileService>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<ILocation, LocationService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            var accountSid = configuration.GetValue<string>("TwilioAccountSID");
            var authToken = configuration.GetValue<string>("TwilioAuthToken");
            TwilioClient.Init(accountSid, authToken);

            var verificationServiceSID = configuration.GetValue<string>("TwilioVerificationServiceSID");
            services.Configure<TwilioVerifySettings>(opts => opts.VerificationServiceSID = verificationServiceSID );

            services.AddTransient<IPhoneVerifyService, PhoneVerifyService>();

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
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/chathub")))
                            {
                                // Read the token out of the query 
                                System.Console.WriteLine($"{accessToken}");
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                }).AddGoogle(options =>
                {
                    //IConfigurationSection googleAuthNSection =
                    //    configuration.GetSection("Authentication:Google");

                    options.ClientId = configuration.GetValue<string>("GoogleOAuthClientId");
                    options.ClientSecret = configuration.GetValue<string>("GoogleOAuthClientSecret");
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("OnlyAdmin",
                    options => {
                        options.RequireAssertion(context => {
                            return context.User.HasClaim(c => (c.Type == ClaimTypes.Role || c.Type == JwtClaimTypes.Role) && c.Value == "Admin");
                        });
                    });
                config.AddPolicy("OnlyModerador", options => {
                    options.RequireAssertion(context => {
                        return context.User.HasClaim(c => (c.Type == ClaimTypes.Role || c.Type == JwtClaimTypes.Role) && c.Value == "Moderador");
                    });
                });
            });

            //services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();

            return services;
        }
    }
}
