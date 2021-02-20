using Application;
using Application.Middlewares;
 
using AutoMapper;
using AutoWrapper;
using Core;
using Data;
using DataAccess;
using Hangfire;
using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application.Services.Cache;
using StackExchange.Redis;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            services.AddCors();

            services.AddControllers();

            services.AddAutoMapper(
                typeof(Application.DependencyInjection).GetTypeInfo().Assembly,
                GetType().Assembly
            );

            services.Configure<EmailSettings>(Configuration.GetSection("emailSettings"));

            services.ConfigureInjections();
            
            services.AddHttpClient();
            services.AddHttpContextAccessor();


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true; // set Modelstate validation manual
            });
            services.AddApplication(Configuration);
            services.AddCore();
            services.AddDataAccess();
            services.AddData(Configuration);
            services
                .ConfigureApplicationCookie(option =>
                {
                    option.LoginPath = "/homeroute/login";
                    option.LogoutPath = "/homeroute/logout";
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            //services.AddSingleton<ICacheService, InMemoryCacheService>();
            var database = Configuration["RedisConnectionString"] ?? "localhost:6379";
            services.AddSingleton<IConnectionMultiplexer>(x =>
                ConnectionMultiplexer.Connect(Configuration.GetValue<string>("RedisConnection")));
            services.AddSingleton<ICacheService, RedisCacheService>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }
            else
            {
                app.UseExceptionHandler("/HomeRoute/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseHangfireDashboard();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
            {
                //UseApiProblemDetailsException = true,
                IsApiOnly = false,
                IgnoreNullValue = false,
                ShowStatusCode = true,
                ShowIsErrorFlagForSuccessfulResponse = true,
                //IgnoreWrapForOkRequests = true,
                EnableExceptionLogging = true,
                EnableResponseLogging = false,
                LogRequestDataOnException = false,
                UseCustomExceptionFormat = false,
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
