using Bank.AzureSearchService.Services;
using Bank.AzureSearchService.Services.Search;
using Bank.AzureSearchService.Services.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.AzureSearchService
{
    public static class AzureSearchRegistration
    {
        public static IServiceCollection AddAzureSearchService(this IServiceCollection services)
        {
            services.AddTransient<IAzureSearch, AzureSearch>();
            services.AddTransient<IAzureUpdater, AzureUpdater>();

            return services;
        }
    }
}
