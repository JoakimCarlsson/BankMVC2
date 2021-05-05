using Bank.Core.Services.Customers;
using Bank.Core.Services.Transactions;
using Bank.Core.Validators.Transfer;
using Bank.Core.ViewModels.Transactions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Bank.Core.Services.Statistics;

namespace Bank.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IValidator<DepositViewModel>, DepositViewModelValidator>();
            services.AddTransient<IValidator<TransferViewModel>, TransferViewModelValidator>();
            services.AddTransient<IValidator<WithdrawViewModel>, WithdrawViewModelValidator>();

            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ITransactionService, TransactionService>();

            return services;
        }
    }
}
