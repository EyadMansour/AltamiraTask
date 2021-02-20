using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureDatabase(configuration);
            return services;
        }
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var server = configuration["DBServer"] ?? "localhost";
            var port = configuration["DBPort"] ?? "1433";
            var user = configuration["DBUser"] ?? "SA";
            var password = configuration["DBPassword"] ?? "Pa55w0rd2020";
            var database = configuration["Database"] ?? "altamiraDb";
            var connectionString = $"Server={server},{port};Database={database};User ID={user};Password={password}";

            //services.AddDbContext<ApplicationDbContext>(
            //   options => options.UseNpgsql(configuration.GetConnectionString("DefaultPostgre")));
            
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));


            return services;
        }
    }
}
