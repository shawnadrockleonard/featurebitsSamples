using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FeatureBits.Core;
using FeatureBits.Data;
using FeatureBits.Data.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FeatureBitWebDev
{
    public class Startup
    {
        private const string FeatureBitsDbConnectionStringKey = "FeatureBitsDbContext";
        private const string FeatureBitsCloudStorageConnectionStringKey = "FeatureBitsAzureTableStorage";
        private IHostingEnvironment EnvironmentHost { get; }

        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment environment)
        {
            EnvironmentHost = environment;
            Configuration = BuildConfiguration(); 

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            System.Diagnostics.Trace.TraceInformation($"===> Environment {environmentName} running.");
        }

        /// <summary>
        /// Build configuration from AppSettings, Environment Variables, Azure Key Valut and (User Secrets - DEV only).
        /// </summary>
        /// <returns><see cref="IConfiguration"/></returns>
        private IConfigurationRoot BuildConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets(typeof(Startup).Assembly);

            if (EnvironmentHost.IsDevelopment())
            {
                // Re-add User secrets so it takes precedent for local development
                configurationBuilder.AddUserSecrets(typeof(Startup).Assembly);
            }

            return configurationBuilder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string featureBitsConnectionString = Configuration.GetConnectionString(FeatureBitsDbConnectionStringKey);
            services.AddDbContext<FeatureBitsEfDbContext>(options => options.UseSqlServer(featureBitsConnectionString));
            services.AddTransient<IFeatureBitsRepo, FeatureBitsEfRepo>((serviceProvider) =>
            {
                DbContextOptionsBuilder<FeatureBitsEfDbContext> options = new DbContextOptionsBuilder<FeatureBitsEfDbContext>();
                options.UseSqlServer(featureBitsConnectionString);
                var context = new FeatureBitsEfDbContext(options.Options);
                return new FeatureBitsEfRepo(context);
            });
            services.AddTransient<IFeatureBitEvaluator, FeatureBitEvaluator>();



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
