using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FirmaBudowlana.Infrastructure.EF;
using FirmaBudowlana.Infrastructure.IoC;

namespace FirmaBudowlana
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
       
        public Startup(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
           
            services.AddDbContext<DBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<RepositoryModule>();
            containerBuilder.RegisterModule<ServiceModule>();
          
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
          

            app.UseMvc(routes =>
            {
                routes.MapRoute
                (name: "default",
                template: "{controller=Home}/{action=Index}/{id?}"
                );

            });
        }
    }
}
