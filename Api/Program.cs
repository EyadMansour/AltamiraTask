using Data;
using Data.Initialize;
using Domain.Entities.Identity;
using Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().BuildLoggerConfiguration().CreateLogger();

            try
            {
                Log.Information("Starting up");
                var host = CreateHostBuilder(args)
                    .Build().Migrate<ApplicationDbContext>();

                //2. Find the service layer within our scope.
                using (var scope = host.Services.CreateScope())
                {
                    //3. Get the instance of BoardGamesDBContext in our services layer

                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<Role>>();

                    //4. Call the DataGenerator to create sample data

                    await Initialize.SeedAsync(context, userManager: userManager, roleManager).ConfigureAwait(false);

                }
                //Continue to run the application
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddJsonFile("Settings/appsettings.json", false, true);
                     config.AddJsonFile($"Settings/appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                         optional: true);
                     config.AddJsonFile("Settings/emailSettings.json", false, true);
                     config.AddJsonFile("Settings/tokenSettings.json", false, true);
                     config.AddCommandLine(args);
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();
                });
    }
}
