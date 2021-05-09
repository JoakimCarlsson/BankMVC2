using Bank.Core.Services.Customers;
using Bank.Core.Services.Statistics;
using Bank.Core.Services.Transactions;
using Bank.Core.Services.User;
using Bank.Core.Validators.Transfer;
using Bank.Core.ViewModels.Transactions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Bank.Core.Validators.Customer;
using Bank.Core.Validators.User;
using Bank.Core.ViewModels.Customers;
using Bank.Core.ViewModels.User;

namespace Bank.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddTransient<IValidator<DepositViewModel>, DepositViewModelValidator>();
            //services.AddTransient<IValidator<TransferViewModel>, TransferViewModelValidator>();
            //services.AddTransient<IValidator<WithdrawViewModel>, WithdrawViewModelValidator>();
            //services.AddTransient<IValidator<UserRegisterViewModel>, UserRegisterViewModelValidator>();
            //services.AddTransient<IValidator<UserEditViewModel>, UserEditViewModelValidator>();

            //services.AddTransient<IValidator<CustomerBaseViewModel>, CustomerBaseViewModelValidator>();
            //services.AddTransient<IValidator<CustomerRegisterViewModel>, CustomerRegisterViewModelValidator>();
            //services.AddTransient<IValidator<CustomerEditViewModel>, CustomerEditViewModelValidator>();

            //services.AddTransient<IStatisticsService, StatisticsService>();
            //services.AddTransient<ICustomerService, CustomerService>();
            //services.AddTransient<ITransactionService, TransactionService>();
            //services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
