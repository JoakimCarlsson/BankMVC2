using Bank.Data;
using Bank.MoneyLaundererBatch.Services;
using Bank.MoneyLaundererBatch.Services.Email;
using Bank.MoneyLaundererBatch.Services.MoneyLaunderer;
using Bank.MoneyLaundererBatch.Services.Report;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.MoneyLaundererBatch
{
    public class Startup
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
            
            services.AddSingleton(typeof(Application));
            services.AddTransient<IMoneyLaundererService, MoneyLaundererService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}