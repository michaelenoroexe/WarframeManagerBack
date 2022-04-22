using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace userRegister
{


    class Program
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(); // Adding CORS Secvices
        }

        static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            // Allowing CORS
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
