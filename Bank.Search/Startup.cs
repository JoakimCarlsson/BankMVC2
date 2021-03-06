using Bank.AzureSearchService;
using Bank.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Search
{
    class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataServices();
            services.AddDatabaseContext(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddAzureSearchService();
        }
    }
}
