using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreReproFor6415
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // HACK Workaround for https://github.com/aspnet/AspNetCore/issues/4437
            app.Use(
                async (context, next) =>
                {
                    await next();

                    if (context.Response.StatusCode == 204)
                    {
                        context.Response.ContentLength = 0;
                    }
                });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error")
                   .UseStatusCodePagesWithReExecute("/error", "?id={0}");
            }

            app.UseMvc();
        }
    }
}
