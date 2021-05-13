using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bank.AzureSearchService.Services;
using Bank.AzureSearchService.Services.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            
            var azureBatch = serviceProvider.GetRequiredService<IAzureUpdater>();
            await azureBatch.RunCustomerUpdateBatchAsync();
            
            // var search = serviceProvider.GetRequiredService<IAzureSearch>();
            // var result = await search.SearchCustomersAsync("Joakim Carlsson", "", 0, 10);
            //var updater = serviceProvider.GetRequiredService<IAzureUpdater>();
            //await updater.UpdateCustomer(new Customer()).ConfigureAwait(false);
        }
    }
}
