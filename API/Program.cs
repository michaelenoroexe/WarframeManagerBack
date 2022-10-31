using Microsoft.AspNetCore;

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
