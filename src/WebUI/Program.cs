using rentasgt.Infrastructure.Identity;
using rentasgt.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using rentasgt.Domain.Entities;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

namespace rentasgt.WebUI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    if (context.Database.IsMySql())
                    {
                        context.Database.Migrate();
                    }                   

                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await ApplicationDbContextSeed.SeedUserRoles(roleManager);
                    await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager);
                    await ApplicationDbContextSeed.SeedSampleDataAsync(context, userManager);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((contextBoundObject, config) =>
                    {
                        if (contextBoundObject.HostingEnvironment.IsProduction())
                        {
                            var buildConfig = config.Build();
                            var vaultUri = $@"https://.vault.azure.net/";
                            var secretClient = new SecretClient(
                            new Uri(vaultUri),
                            new DefaultAzureCredential());
                            config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
                        }
                        
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
