using System;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;
using Bank.Data;
using Bank.Data.Models;
using Bank.Search.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IAzureUpdater = Bank.AzureSearchService.Services.IAzureUpdater;

namespace Bank.Search
{
    class Program
    {
        static async Task Main(string[] args)
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
            //await updater.UpdateCustomer(new Customer()).ConfigureAwait(false);
        }
    }
}
