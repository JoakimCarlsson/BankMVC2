﻿using Bank.Core.Repository.AccountRep;
using Bank.Core.Repository.Base;
using Bank.Core.Repository.CustomerRep;
using Bank.Core.Repository.DispositionRep;
using Bank.Core.Repository.TranasctionsRep;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Bank.Core.Validators;
using Bank.Core.ViewModels.Transactions;
using FluentValidation;

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
            services.AddScoped<IDispositionRepository, DispositionRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IValidator<DepositViewModel>, DepositValidator>();
            return services;
        }
    }
}
