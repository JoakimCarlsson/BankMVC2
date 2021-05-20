using  System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.MoneyLaundererBatch
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //https://benfoster.io/blog/how-to-configure-kestrel-urls-in-aspnet-core-rc2
            //< ItemGroup >     < Content Include = "appsettings.json" >       < CopyToOutputDirectory > PreserveNewest </ CopyToOutputDirectory >     </ Content >   </ ItemGroup >

               var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceCollection = new ServiceCollection();

            Startup startup = new Startup(configuration);
            startup.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            await serviceProvider.GetRequiredService<Application>().RunAsync();
        }
    }
}