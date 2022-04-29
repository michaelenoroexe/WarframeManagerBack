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


namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        static IMongoDatabase db = DBClient.db;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddAuthorization();

            services.AddCors(); // Adding CORS Secvices

            //JwtAuth
            //services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        }

        public void Configure(IApplicationBuilder app)
        {
            Console.WriteLine(db);
            // Configure the HTTP request pipeline.
            app.UseDeveloperExceptionPage();       
            app.UseRouting();

            // Allowing CORS
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
    //class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    //{
    //    private readonly IOptions<JwtAuthentication> _jwtAuthentication;

    //    public ConfigureJwtBearerOptions(IOptions<JwtAuthentication> jwtAuthentication)
    //    {
    //        _jwtAuthentication = jwtAuthentication ?? throw new System.ArgumentNullException(nameof(jwtAuthentication));
    //    }

    //    public void PostConfigure(string name, JwtBearerOptions options)
    //    {
    //        var jwtAuthentication = _jwtAuthentication.Value;

    //        options.ClaimsIssuer = jwtAuthentication.ValidIssuer;
    //        options.IncludeErrorDetails = true;
    //        options.RequireHttpsMetadata = true;
    //        options.TokenValidationParameters = new TokenValidationParameters
    //        {
    //            ValidateActor = true,
    //            ValidateIssuer = true,
    //            ValidateAudience = true,
    //            ValidateLifetime = true,
    //            ValidateIssuerSigningKey = true,
    //            ValidIssuer = jwtAuthentication.ValidIssuer,
    //            ValidAudience = jwtAuthentication.ValidAudience,
    //            IssuerSigningKey = jwtAuthentication.SymmetricSecurityKey,
    //            NameClaimType = ClaimTypes.NameIdentifier
    //        };
    //    }
    //}
}