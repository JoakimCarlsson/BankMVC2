using System.IO;
using System.Threading.Tasks;
using Bank.AzureSearchService.Services;
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
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceCollection = new ServiceCollection();

            Startup startup = new Startup(configuration);
            startup.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            var azureBatch = serviceProvider.GetRequiredService<IAzureUpdater>();
            await azureBatch.RunCustomerUpdateBatchAsync().ConfigureAwait(false);
        }
    }
}
