using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.IO;
namespace Demo
{
    public static class IocManager
    {
        static IServiceProvider _serviceProvider;

        public static IServiceCollection Services { get; private set; }

        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = Services.BuildServiceProvider();
                }

                return _serviceProvider;
            }
        }

        public static IConfiguration Configuration { get; private set; }

        static IocManager()
        {
            Configuration = InitConfiguration();

            Services = new ServiceCollection();
            Services.AddSingleton(Configuration);
        }


        static IConfiguration InitConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                // ReSharper disable once StringLiteralTypo
                .AddJsonFile("appsettings.json",false,true);
            var configuration = configurationBuilder.Build();
            return configuration;
        }

        public static void Build()
        {
            _serviceProvider = Services.BuildServiceProvider();
        }

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            if (_serviceProvider != null)
            {
                return;
            }

            _serviceProvider = serviceProvider;
        }

    }
}