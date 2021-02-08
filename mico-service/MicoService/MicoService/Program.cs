using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicoService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                Console.WriteLine("ConfigureWebHostDefaults");
                                webBuilder.UseStartup<Startup>();
                            })
                            .ConfigureServices(builder =>
                            {
                                Console.WriteLine("ConfigureServices1");
                            })
                            .ConfigureAppConfiguration(builder =>
                            {
                                Console.WriteLine("ConfigureAppConfiguration");
                            })

                            .ConfigureHostConfiguration(builder =>
                            {

                                Console.WriteLine("ConfigureHostConfiguration");
                            }

            );

    }
}
