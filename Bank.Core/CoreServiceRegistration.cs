using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bank.Core.Repository;
using Bank.Core.Repository.AccountRep;
using Bank.Core.Repository.Base;
using Bank.Core.Repository.CustomerRep;
using Bank.Core.Repository.TranasctionsRep;
using Bank.Core.Services.CustomerServ;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            //repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            //services
            //services.AddScoped<ICustomerService, CustomerService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
