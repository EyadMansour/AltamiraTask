using Application.Middlewares;
using Application.Services;
using Data;
using Data.Common;
using Domain.Entities.Identity;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using System;
using System.Linq;
using Hangfire.SqlServer;
using DataAccess.Repository.Companies;
using Application.Services.Entities.Companies;
using Application.Services.Entities.Identity;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,
            IConfiguration configuration)
        {
            //services.ConfigureAuthentication(configuration);
            services.ConfigureAuthorization();
            //services.ConfigureHangfire(configuration);
            services.ConfigureAppCookie();
            services.ConfigureIdentity();
            services.ConfigureServices();

            return services;
        }
        private static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(
                options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredUniqueChars = 0;
                }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            return services;
        }
        public static IServiceCollection ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddHangfire(conf => conf
            //    .UsePostgreSqlStorage(configuration.GetConnectionString("DefaultPostgre"),
            //        new PostgreSqlStorageOptions
            //        {
            //            SchemaName = SchemaNames.HangfireTableSchemaName
            //        }));
            var x = configuration.GetConnectionString("DefaultSQL");
            services.AddHangfire(conf => conf
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultSQL"),
                    new SqlServerStorageOptions()
                    {
                        SchemaName = SchemaNames.HangfireTableSchemaName
                    }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            return services;
        }
        public static IServiceCollection ConfigureAppCookie(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                options.LogoutPath = "/Logout";
                options.LoginPath = "/Login"; // Set here your login path.
                options.AccessDeniedPath = "/Error/?errCode=404"; // set here your access denied path.

                options.SlidingExpiration = true;

            });
            return services;
        }

        private static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
                var categories = context.PermissionCategoryPermissions
                    .Include(c => c.Permission)
                    .Include(c => c.Category);


                foreach (var permissionCategory in categories)
                {
                    //Usage : user_add
                    options.AddPolicy(permissionCategory.Category.Label.ToLower() + "_" + permissionCategory.Permission.Label.ToLower(),
                        policy => policy.Requirements.Add(new PermissionRequirement(
                            new PermissionRequirementModel(permissionCategory.PermissionId, permissionCategory.CategoryId)
                        )));

                }

                var permissions = context.Permissions.Where(c => c.IsDirective).ToList();
                foreach (var permission in permissions)
                {
                    //Usage : admin
                    options.AddPolicy(permission.Label.ToLower(),
                        policy => policy.Requirements.Add(new PermissionRequirement(
                            new PermissionRequirementModel(permission.Label)
                        )));
                }

            });
            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {

            #region identity ve configler
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<RoleService>();
            services.AddScoped<UploadService>();
            services.AddScoped<UploadService>();
            services.AddScoped<ExportService>();
            services.AddScoped<EmailService>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            #endregion

            services.AddScoped<ICompanyService, CompanyService>();


            return services;
        }
    }
}
