using Bank.AzureSearchService.Services;
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
