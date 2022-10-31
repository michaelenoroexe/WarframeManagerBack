using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using API.Repositories;
using API.Models;
using API;

namespace API
{


    class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();   
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // Using startup to configure application parameters
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}
