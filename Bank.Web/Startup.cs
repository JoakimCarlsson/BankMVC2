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
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            services.AddCoreServices();
            services.AddDatabaseContext(Configuration);
            services.AddDataServices();

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
