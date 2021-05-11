using System;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using Bank.Data;
using Bank.Search.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Search
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"C:\Users\Joakim\source\repos\Bank\Bank.Search\config.json")//todo fix me path.
                .Build();

            var serviceCollection = new ServiceCollection();

            Startup startup = new Startup(configuration);
            startup.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            //var updater = serviceProvider.GetRequiredService<IAzureUpdater>();
            //updater.Run();

            var searcher = serviceProvider.GetRequiredService<IAzureSearcher>();
            searcher.Run();
        }
    }
}
