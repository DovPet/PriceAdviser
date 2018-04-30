using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceAdvisor.Persistence;
using Hangfire;
using AutoMapper;
using PriceAdvisor.Core;
using Swashbuckle.AspNetCore.Swagger;

namespace PriceAdvisor
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
           var builder = new ConfigurationBuilder()
                        .SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional:true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                    Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
            c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
            services.AddAutoMapper();
            services.AddHangfire(configuration => { configuration.UseSqlServerStorage(Configuration.GetConnectionString("Default"));
                });
            services.AddAutoMapper();
            services.AddDbContext<PriceAdvisorDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAdministrationRepository, AdministrationRepository>();
            services.AddScoped<IEshopRepository, EshopRepository>();
            services.AddScoped<IExportRepository, ExportRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
             GlobalConfiguration.Configuration.UseSqlServerStorage(Configuration.GetConnectionString("Default"));
              app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
              app.UseSwagger();
             app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
             app.UseHangfireServer();
            
            app.UseHangfireDashboard();
            if (env.IsDevelopment())
            {
               
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

               
                
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
           
        }
    }
}
