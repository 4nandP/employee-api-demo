using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Employee.Api
{
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

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return System.IO.Path.Combine(basePath, fileName);
            }
        }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
            .AddControllersAsServices()
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                });
            services.AddVersionedApiExplorer(options => { options.GroupNameFormat = "'v'V"; options.SubstituteApiVersionInUrl = true; });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(XmlCommentsFilePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseSwagger(options => {
                //options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                //{
                //    swaggerDoc.BasePath = $"/v{swaggerDoc.Info.Version}";
                //});
                //options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                //{
                //    foreach(var path in swaggerDoc.Paths)
                //    {
                //        var operations = new[] { path.Value.Delete, path.Value.Get, path.Value.Patch, path.Value.Post, path.Value.Put };
                //        foreach(var op in operations)
                //        {
                //            var apiVersionParam = op?.Parameters?.FirstOrDefault(p => p.Name == "api-version");
                //            if (apiVersionParam != null)
                //            {
                //                op.Parameters.Remove(apiVersionParam);
                //            }
                //        }
                //    }
                //});
            });
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                    options.DocumentTitle = "Employee API";
                    options.InjectStylesheet("/swagger-ui/custom.css");
                });
        }
    }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
          this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                  description.GroupName,
                    new Info()
                    {
                        Title = $"Employee API",
                        Version = description.ApiVersion.ToString(),
                    });
            }
        }
    }
}
