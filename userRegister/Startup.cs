﻿using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Newtonsoft.Json;
using API.Repositories;
using API.Logger;
using System.Text.Json;
using API.Models;
using API.Models.Interfaces;
using API.Models.Service;
using API.Models.Common;
using Shared;
using BufferUserRequests;
using API.Models.Common.ItemComp;
using API.Models.UserWork.Interfaces;
using API.Models.UserWork.Getter;
using UserValidation.JWTValidation;
using UserValidation.LoginPasswordValidator;
using API.Models.UserWork.Setter;
using API.Models.UserWork;
using UserValidation;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
            services.AddAuthorization();
            #region Dependency Injection.
            #region Database collections.
            var DBclient = DBClient.GetDBClient();
            IMongoDatabase DB = DBclient.Db;
            services.AddSingleton(DB.GetCollection<Item>("Components"));
            services.AddSingleton(DB.GetCollection<FullUser>("Users"));
            services.AddSingleton(DB.GetCollection<UserResources>("UsersResources"));
            services.AddSingleton(DB.GetCollection<UserInfo>("UsersInfo"));
            services.AddSingleton(DB.GetCollection<Planet>("Planets"));
            services.AddSingleton(DB.GetCollection<Restype>("Types"));
            // Instantiate item collection service.
            services.AddSingleton<ICollectionProvider, CollectionProvider>();
            #endregion
            #region Functional dependency.
            services.AddLogging(conf => conf.AddLog(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")));
            services.AddSingleton<IPasswordEqualityComparer, Hash>();
            services.AddSingleton<IPasswordHasher, Hash>();
            #region Buffer
            services.AddSingleton<UsersChangeBuffer>();
            services.AddSingleton<IBufferChecker, UserBufferChecker>();
            services.AddSingleton<IBufferChanger, UserBufferChanger>();
            services.AddSingleton<DBGetter, UserDBGetter>();
            services.AddSingleton<IUserInfoGetter, UserInfoGetter>();
            #endregion
            #region User validator
            services.AddSingleton<IUserValidator<ClaimsPrincipal>, JWTUserValidator>();
            services.AddSingleton<IUserValidator<(string, string)>, LogPassUserValidator >();
            #endregion
            services.AddSingleton<IGetData, GetDataRepository>();
            services.AddSingleton<IChangeData, ProfileUpdateRepository>();
            services.AddSingleton<IUserManager, UserRepository>();
            #endregion
            // Adding CORS Secvices.
            services.AddCors();
            #endregion
            //JwtAuth
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Show pieces of validation tocken
                        ValidateActor = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JwtAuthentication.ValidIssuer,
                        ValidAudience = JwtAuthentication.ValidAudience,
                        IssuerSigningKey = JwtAuthentication.SymmetricSecurityKey,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseRouting();

            // Allowing CORS
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
