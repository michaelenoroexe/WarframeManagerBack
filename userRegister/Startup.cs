using System.Security.Claims;
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

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        static IMongoDatabase db = DBClient.Db;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
            services.AddAuthorization();
            // Dependency Injection
            services.AddLogging(conf => conf.AddLog(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")));
            services.AddSingleton<GetDataRepository>();
            services.AddSingleton<ProfileUpdateRepository>();
            services.AddSingleton<UserRepository>();
            services.AddCors(); // Adding CORS Secvices

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
