using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MaaltijdApplicatie.Models.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Users.Models.Context;
using Microsoft.AspNetCore.Identity;
using MaaltijdApplicatie.Models.Domain;
using System.Globalization;

namespace MaaltijdApplicatie {

    public class Startup {

        public Startup(IConfiguration configuration) =>
            Configuration = configuration;
        public IConfiguration Configuration { get; }

        // Adding services to the container
        public void ConfigureServices(IServiceCollection services) {

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:MaaltijdApplicatieIdentity:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            // Add meal repo implementation to container
            services.AddTransient<IMealRepository, DbMealRepository>();

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
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
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
