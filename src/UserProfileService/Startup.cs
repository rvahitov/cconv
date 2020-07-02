using Autofac;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using UserProfileService.Infrastructure.Modules;

namespace UserProfileService
{
    public class Startup
    {
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddControllers();
        }

        [ UsedImplicitly ]
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints( endpoints => { endpoints.MapControllers(); } );
        }

        [ UsedImplicitly ]
        public void ConfigureContainer( ContainerBuilder builder )
        {
            builder.RegisterModule( new MainModule() );
        }
    }
}