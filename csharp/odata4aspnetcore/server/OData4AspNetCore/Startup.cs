using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using OData4AspNetCore.Models;

namespace OData4AspNetCore
{
    public class Startup
    {
        private string baseDir;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            baseDir = env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<MoviesContext>(opt => opt.UseInMemoryDatabase("MoviesCore"));
            var connString = Configuration.GetConnectionString("MoviesCoreDatabase");

            string appDataDir = System.IO.Path.Combine(baseDir, "App_Data");
            Directory.CreateDirectory(appDataDir);

            connString = connString.Replace("|DataDirectory|", appDataDir); // this is needed if you are using ef core.

            services.AddDbContext<MoviesContext>(options =>
                options.UseSqlServer(connString));

            services.AddOData();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MoviesContext>();
                try
                {
                    context.Database.EnsureCreated();
                }
                catch
                {
                    throw;
                }
            }

            app.UseMvc(b =>
            {
                b.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                b.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<Movie>("Movies")
                .EntityType.Collection.Action("UpdateAll").CollectionParameter<Movie>("value");

            return builder.GetEdmModel();
        }

    }
}
