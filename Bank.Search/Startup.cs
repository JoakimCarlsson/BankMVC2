using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Data;
using Bank.Search.Azure;
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
            services.AddTransient<IAzureUpdater, AzureUpdater>();
            services.AddTransient<IAzureSearcher, AzureSearcher>();
        }
    }
}
