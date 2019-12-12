﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VOD.Database;
using VOD.Domain.Entities;
using AutoMapper;
using VOD.Common.AutoMapper;
using VOD.Domain.Interfaces;
using VOD.Domain.Services;
using System.Net.Http;
using VOD.Domain.Services.Services;
using VOD.Common;
using VOD.Common.Constants;
using VOD.Common.Services;
using Microsoft.AspNetCore.Identity.UI;
using VOD.Domain.Interfaces.Services;

namespace VOD.Admin
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
            /*services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });*/

            services.AddDbContext<VODContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<VODUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<VODContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddHttpClient(AppConstants.HttpClientName, client =>
            {
                client.BaseAddress = new Uri("http://localhost:6600"); //TODO: move to configuration
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
            }).ConfigurePrimaryHttpMessageHandler(handler =>
                new HttpClientHandler()
                {
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip
                });

            services.AddRazorPages()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddScoped<IDbReadService, DbReadService>();
            services.AddScoped<IDbWriteService, DbWriteService>();
            services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IAdminService, AdminEFService>();
            services.AddScoped<IAdminService, AdminAPIService>();
            services.AddScoped<IAdminGrpcService, AdminGrpcService>();
            services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();
            //services.AddScoped<IAdminCoursesService, AdminCoursesService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddAutoMapper(typeof(AdminMappingProfile).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
