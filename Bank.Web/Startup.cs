using System.Reflection;
using Bank.Core;
using Bank.Core.Services.Customers;
using Bank.Core.Services.Statistics;
using Bank.Core.Services.Transactions;
using Bank.Core.Services.User;
using Bank.Core.Validators.Customer;
using Bank.Core.Validators.Transfer;
using Bank.Core.Validators.User;
using Bank.Core.ViewModels.Customers;
using Bank.Core.ViewModels.Transactions;
using Bank.Core.ViewModels.User;
using Bank.Data.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bank.Data;
using Bank.Data.Repositories.Account;
using Bank.Data.Repositories.Base;
using Bank.Data.Repositories.Customer;
using Bank.Data.Repositories.Disposition;
using Bank.Data.Repositories.Transaction;
using FluentValidation;

namespace Bank.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddAutoMapper(typeof(Startup)); //Auto mapper

            services.AddTransient<IValidator<DepositViewModel>, DepositViewModelValidator>();
            services.AddTransient<IValidator<TransferViewModel>, TransferViewModelValidator>();
            services.AddTransient<IValidator<WithdrawViewModel>, WithdrawViewModelValidator>();
            services.AddTransient<IValidator<UserRegisterViewModel>, UserRegisterViewModelValidator>();
            services.AddTransient<IValidator<UserEditViewModel>, UserEditViewModelValidator>();

            services.AddTransient<IValidator<CustomerBaseViewModel>, CustomerBaseViewModelValidator>();
            services.AddTransient<IValidator<CustomerRegisterViewModel>, CustomerRegisterViewModelValidator>();
            services.AddTransient<IValidator<CustomerEditViewModel>, CustomerEditViewModelValidator>();

            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IUserService, UserService>();

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IDispositionRepository, DispositionRepository>();

            services.AddCoreServices(); //bank.core
            //services.AddDataServices(Configuration); //bank.data

            services.AddResponseCaching();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews().AddFluentValidation();

            //services.AddMvc()
            //    .AddFluentValidation(fvc =>
            //        fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
