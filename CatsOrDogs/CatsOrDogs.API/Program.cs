using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CatsOrDogs.API
{
    /// <summary/>
    public static class Program
    {
        /// <summary/>
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        /// <summary/>
        private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}