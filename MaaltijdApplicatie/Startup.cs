using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MaaltijdApplicatie.Models.Repositories;

namespace MaaltijdApplicatie {

    public class Startup {

        // Adding services to the container
        public void ConfigureServices(IServiceCollection services) {

            // Add meal repo implementation to container
            services.AddTransient<IMealRepository, FakeMealRepository>();

            // Add MVC
            services.AddMvc();

        }

        // Configure http request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {

            // Use developer exception pages if the application is in development mode
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();

            // Configure http request routes
            app.UseMvc(routes => {



            });

        }

    }

}
