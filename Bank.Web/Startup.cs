using Bank.Data;
using Bank.Data.Data;
using Bank.Data.Repositories.Base;
using Bank.Web.Services.Customers;
using Bank.Web.Services.Statistics;
using Bank.Web.Services.Transactions;
using Bank.Web.Services.User;
using Bank.Web.Validators.Customer;
using Bank.Web.Validators.Transfer;
using Bank.Web.Validators.User;
using Bank.Web.ViewModels.Customers;
using Bank.Web.ViewModels.Transactions;
using Bank.Web.ViewModels.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddAutoMapper(typeof(Startup));

            services.AddDatabaseContext(Configuration);
            services.AddDataServices();

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



            services.AddResponseCaching();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews().AddFluentValidation();
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
