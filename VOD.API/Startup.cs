﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VOD.API.Services;
using VOD.Common.AutoMapper;
using VOD.Common.Constants;
using VOD.Database;
using VOD.Domain.Entities;
using VOD.Domain.Interfaces;
using VOD.Domain.Services;

namespace VOD.API
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
            services.AddDbContext<VODContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<VODUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<VODContext>();

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //services.AddRouting
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Latest);

            /* services.AddMvc(options =>
             {
                 options.OutputFormatters.Insert(0, new XmlSerializerOutputFormatter());
                 options.InputFormatters.Insert(0, new XmlSerializerInputFormatter(options));
             }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);*/

            //services.AddMvc().AddXmlSerializerFormatters().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAutoMapper(typeof(AdminMappingProfile));
            services.AddScoped<IDbReadService, DbReadService>();
            services.AddScoped<IDbWriteService, DbWriteService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminEFService>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var signingKey = new SymmetricSecurityKey(Convert.FromBase64String(Configuration["Jwt:SigningSecret"]));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.Zero
                };
                options.RequireHttpsMetadata = false;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Roles.VODUser, policy => policy.RequireClaim(Roles.VODUser, "true"));
                options.AddPolicy(Roles.Admin, policy => policy.RequireClaim(Roles.Admin, "true"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseStatusCodePages();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                //app.UseStatusCodePages();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
