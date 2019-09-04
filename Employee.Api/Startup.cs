﻿using JsonFlatFileDataStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Employee.Api
{
    /// <summary>
    /// Application Bootstrap
    /// </summary>
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the hosting environment.
        /// </summary>
        /// <value>
        /// The hosting environment.
        /// </value>
        public IHostingEnvironment HostingEnvironment { get; }

        /// <summary>
        /// Gets the XML comments file path.
        /// </summary>
        /// <value>
        /// The XML comments file path.
        /// </value>
        private static List<string> XmlCommentsFilePaths
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var docFiles = System.IO.Directory.GetFiles(basePath, "*.xml");
                return docFiles.ToList();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDataStore(services);

            services.AddSingleton<V1.Application.Queries.IEmployeeQueries, V1.Application.Queries.EmployeeQueries>();
            services.AddSingleton<V2.Application.Queries.IEmployeeQueries, V2.Application.Queries.EmployeeQueries>();

            //services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
            .AddControllersAsServices()
            .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options => XmlCommentsFilePaths.ForEach(x => options.IncludeXmlComments(x, true)));
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="provider">The provider.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = $"public,max-age={durationInSeconds}";
                }
            });

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                    options.DocumentTitle = "Employee API";
                    options.InjectStylesheet("/swagger-ui/custom.css");
                    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                    options.DefaultModelsExpandDepth(0);
                });
        }

        /// <summary>
        /// Configures the data store.
        /// </summary>
        /// <param name="services">The services.</param>
        private static void ConfigureDataStore(IServiceCollection services)
        {
            var dataStoreFilePath = System.IO.Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "datastore.json");
            var dataStore = new DataStore(dataStoreFilePath, keyProperty: "id", reloadBeforeGetCollection: true).SeedData();

            services.AddSingleton<IDataStore>(dataStore);
        }
    }
}
