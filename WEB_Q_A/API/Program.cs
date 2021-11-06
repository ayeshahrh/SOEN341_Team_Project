using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            travis_error_test.Test();
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                 var context = services.GetRequiredService<DataContext>();
                 var userManager = services.GetRequiredService<UserManager<AppUser>>();
                 var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                 await context.Database.MigrateAsync();
                 await Seed.SeedUsers(userManager, roleManager);
                 await Seed.SeedQuestions(context);
                 await Seed.SeedAnswers(context);
            }
            catch (System.Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
