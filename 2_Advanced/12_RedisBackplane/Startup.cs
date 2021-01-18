using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Scaffold.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // STEP 1: connection string to redis
            //    - Get from configuration! 
            var connectionString = "";
            
            // STEP 2: Add StackExchange Redis helper
            services.AddSignalR().AddStackExchangeRedis(connectionString, configure => {
                configure.Configuration.ChannelPrefix = "signalr";
                configure.Configuration.DefaultDatabase = 5;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseDefaultFiles(); //index.html
            app.UseStaticFiles();

            app.UseEndpoints(configure => {
                configure.MapHub<BackgroundColorHub>("/hub/background");
            });
        }
    }
}
