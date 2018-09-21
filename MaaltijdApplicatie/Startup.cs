using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MaaltijdApplicatie.Models.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Users.Models.Context;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using MaaltijdApplicatie.Models.Context;

namespace MaaltijdApplicatie {

    public class Startup {

        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        // Adding services
        public void ConfigureServices(IServiceCollection services) {

            // Add domain database
            services.AddDbContext<AppDomainDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:MealAppDomainDb:ConnectionString"]));

            // Add identity database
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:MealAppIdentityDb:ConnectionString"]));

            // Add identity support
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            // Meal repo injection
            services.AddTransient<IMealRepository, DbMealRepository>();
            // Student repo injection
            services.AddTransient<IStudentRepository, DbStudentRepository>();

            // Add MVC
            services.AddMvc();

        }

        // Configure http request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {

            // Setting culture
            var cultureInfo = new CultureInfo("nl");
            cultureInfo.NumberFormat.CurrencySymbol = "€";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            // Use developer exception pages if the application is in development mode
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();

            // Use identity
            app.UseAuthentication();

            // Configure http request routes
            app.UseMvc(routes => {

                routes.MapRoute(
                name: null,
                template: "",
                defaults: new {
                    controller = "Meal",
                    action = "List"
                });

                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");

            });

        }

    }

}
