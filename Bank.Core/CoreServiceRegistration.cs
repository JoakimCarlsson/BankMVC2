using Bank.Core.Repository.AccountRep;
using Bank.Core.Repository.Base;
using Bank.Core.Repository.CustomerRep;
using Bank.Core.Repository.DispositionRep;
using Bank.Core.Repository.TranasctionsRep;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Bank.Core.Data;
using Bank.Core.Validators;
using Bank.Core.Validators.Transfer;
using Bank.Core.ViewModels.Transactions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bank.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IDispositionRepository, DispositionRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IValidator<DepositViewModel>, DepositViewModelValidator>();
            services.AddTransient<IValidator<TransferViewModel>, TransferViewModelValidator>();
            services.AddTransient<IValidator<WithdrawViewModel>, WithdrawViewModelValidator>();

            //Startup.cs
            //services.AddControllersWithViews().AddFluentValidation(s =>
            //{
            //    s.RegisterValidatorsFromAssemblyContaining<Startup>();
            //});
            return services;
        }
    }
}
