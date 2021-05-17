using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.MoneyLaundererBatch
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"C:\Users\Joakim\source\repos\Bank\Bank.MoneyLaundererBatch\appsettings.json")//todo fix me path.
                .Build();

            var serviceCollection = new ServiceCollection();

            Startup startup = new Startup(configuration);
            startup.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}